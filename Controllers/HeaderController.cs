using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProUygulama.Api.Data;
using ProUygulama.Api.Dtos;
using ProUygulama.Api.Entities;

namespace ProUygulama.Api.Controllers;

[ApiController]
[Route("api/header")]
public class HeaderController : ControllerBase
{
    private readonly AppDbContext _db;

    public HeaderController(AppDbContext db)
    {
        _db = db;
    }

    // ======================================================
    // GET ACTIVE HEADER (FRONTEND)
    // ======================================================
    [HttpGet]
    public async Task<ActionResult<HeaderResponseDto>> GetActiveHeader()
    {
        var header = await _db.HeaderContents
            .Include(x => x.BackgroundVideo)
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        if (header == null)
            return NotFound("Aktif header bulunamadı.");

        return Ok(new HeaderResponseDto
        {
            Id = header.Id,
            CompanyName = header.CompanyName,
            Slogan = header.Slogan,
            PrimaryButtonText = header.PrimaryButtonText,
            PrimaryButtonUrl = header.PrimaryButtonUrl,
            SecondaryButtonText = header.SecondaryButtonText,
            SecondaryButtonUrl = header.SecondaryButtonUrl,
            BackgroundVideoUrl = header.BackgroundVideo?.PublicUrl
        });
    }

    // ======================================================
    // CREATE HEADER (ADMIN)
    // ======================================================
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<HeaderResponseDto>> Create(
        [FromBody] CreateHeaderDto dto)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync();

        try
        {
            var actives = await _db.HeaderContents
                .Where(x => x.IsActive)
                .ToListAsync();

            foreach (var h in actives)
                h.IsActive = false;

            var header = new HeaderContent
            {
                Id = Guid.NewGuid(),
                CompanyName = dto.CompanyName,
                Slogan = dto.Slogan,
                PrimaryButtonText = dto.PrimaryButtonText,
                PrimaryButtonUrl = dto.PrimaryButtonUrl,
                SecondaryButtonText = dto.SecondaryButtonText,
                SecondaryButtonUrl = dto.SecondaryButtonUrl,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.HeaderContents.Add(header);
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new HeaderResponseDto
            {
                Id = header.Id,
                CompanyName = header.CompanyName,
                Slogan = header.Slogan,
                PrimaryButtonText = header.PrimaryButtonText,
                PrimaryButtonUrl = header.PrimaryButtonUrl,
                SecondaryButtonText = header.SecondaryButtonText,
                SecondaryButtonUrl = header.SecondaryButtonUrl,
                BackgroundVideoUrl = null
            });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // ======================================================
    // SET BACKGROUND VIDEO
    // ======================================================
    [Authorize(Roles = "Admin")]
    [HttpPost("{id:guid}/set-video")]
    public async Task<IActionResult> SetBackgroundVideo(
        Guid id,
        [FromBody] SetHeaderVideoDto dto)
    {
        var header = await _db.HeaderContents.FirstOrDefaultAsync(x => x.Id == id);
        if (header == null)
            return NotFound("Header bulunamadı.");

        if (!header.IsActive)
            return BadRequest("Sadece aktif header'a video bağlanabilir.");

        var media = await _db.MediaFiles.FirstOrDefaultAsync(x => x.Id == dto.MediaFileId);
        if (media == null)
            return NotFound("Media bulunamadı.");


        header.BackgroundVideoId = media.Id;
        await _db.SaveChangesAsync();

        return Ok(new { message = "Header dosyası güncellendi." });
    }

    // ======================================================
    // UPDATE HEADER
    // ======================================================
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateHeaderDto dto)
    {
        var header = await _db.HeaderContents.FirstOrDefaultAsync(x => x.Id == id);
        if (header == null)
            return NotFound("Header bulunamadı.");

        if (!header.IsActive)
            return BadRequest("Sadece aktif header güncellenebilir.");

        header.CompanyName = dto.CompanyName;
        header.Slogan = dto.Slogan;
        header.PrimaryButtonText = dto.PrimaryButtonText;
        header.PrimaryButtonUrl = dto.PrimaryButtonUrl;
        header.SecondaryButtonText = dto.SecondaryButtonText;
        header.SecondaryButtonUrl = dto.SecondaryButtonUrl;

        await _db.SaveChangesAsync();

        return Ok(new { message = "Header güncellendi." });
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var header = await _db.HeaderContents
            .FirstOrDefaultAsync(x => x.Id == id);

        if (header == null)
            return NotFound("Header bulunamadı.");

        _db.HeaderContents.Remove(header);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Header kalıcı olarak silindi.",
            headerId = id
        });
    }
}
