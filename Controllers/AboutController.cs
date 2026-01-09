using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProUygulama.Api.Data;
using ProUygulama.Api.Dtos;
using ProUygulama.Api.Entities;

namespace ProUygulama.Api.Controllers;

[ApiController]
[Route("api/about")]
public class AboutController : ControllerBase
{
    private readonly AppDbContext _db;

    public AboutController(AppDbContext db)
    {
        _db = db;
    }

    // ======================================================
    // GET ACTIVE ABOUT (FRONTEND)
    // ======================================================
    [HttpGet]
    public async Task<ActionResult<AboutResponseDto>> GetActive()
    {
        var about = await _db.AboutContents
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        if (about == null)
            return NotFound("Aktif Hakkımızda içeriği bulunamadı.");

        return Ok(new AboutResponseDto
        {
            Id = about.Id,
            Title = about.Title,
            ContentHtml = about.ContentHtml
        });
    }

    // ======================================================
    // CREATE ABOUT (ADMIN)
    // Eski aktifleri otomatik pasif yapar
    // ======================================================
    [HttpPost]
    public async Task<ActionResult<AboutResponseDto>> Create(
        [FromBody] CreateAboutDto dto)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync();

        try
        {
            var actives = await _db.AboutContents
                .Where(x => x.IsActive)
                .ToListAsync();

            foreach (var a in actives)
                a.IsActive = false;

            var about = new AboutContent
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                ContentHtml = dto.ContentHtml,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.AboutContents.Add(about);
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new AboutResponseDto
            {
                Id = about.Id,
                Title = about.Title,
                ContentHtml = about.ContentHtml
            });
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // ======================================================
    // UPDATE ABOUT (ADMIN)
    // ======================================================
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateAboutDto dto)
    {
        var about = await _db.AboutContents
            .FirstOrDefaultAsync(x => x.Id == id);

        if (about == null)
            return NotFound("Hakkımızda içeriği bulunamadı.");

        if (!about.IsActive)
            return BadRequest("Sadece aktif içerik güncellenebilir.");

        about.Title = dto.Title;
        about.ContentHtml = dto.ContentHtml;
        about.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(new { message = "Hakkımızda içeriği güncellendi." });
    }

    // ======================================================
    // HARD DELETE ABOUT (ADMIN)
    // ======================================================
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var about = await _db.AboutContents
            .FirstOrDefaultAsync(x => x.Id == id);

        if (about == null)
            return NotFound("Hakkımızda içeriği bulunamadı.");

        _db.AboutContents.Remove(about);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Hakkımızda içeriği silindi." });
    }
}
