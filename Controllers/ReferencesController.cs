using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProUygulama.Api.Data;
using ProUygulama.Api.Dtos;
using ProUygulama.Api.Entities;

namespace ProUygulama.Api.Controllers;

[ApiController]
[Route("api/references")]
public class ReferencesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ReferencesController(AppDbContext db)
    {
        _db = db;
    }

    // =====================================
    // FRONTEND
    // GET: /api/references
    // =====================================
    [HttpGet]
    public async Task<IActionResult> GetActive()
    {
        var list = await _db.ReferenceItems
            .Include(x => x.LogoMedia)
            .Where(x => x.IsActive)
            .OrderBy(x => x.Order)
            .Select(x => new ReferenceResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                LogoUrl = x.LogoMedia != null ? x.LogoMedia.PublicUrl : null,
                Order = x.Order
            })
            .ToListAsync();

        return Ok(list);
    }

    // =====================================
    // ADMIN LIST
    // GET: /api/references/admin
    // =====================================
    [HttpGet("admin")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _db.ReferenceItems
            .Include(x => x.LogoMedia)
            .OrderBy(x => x.Order)
            .ToListAsync());
    }

    // =====================================
    // CREATE
    // POST: /api/references
    // =====================================
    [HttpPost]
    public async Task<IActionResult> Create(CreateReferenceDto dto)
    {
        var entity = new ReferenceItem
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            LogoMediaId = dto.LogoMediaId,
            Order = dto.Order,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _db.ReferenceItems.Add(entity);
        await _db.SaveChangesAsync();

        return Ok(entity);
    }

    // =====================================
    // UPDATE
    // PUT: /api/references/{id}
    // =====================================
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateReferenceDto dto)
    {
        var entity = await _db.ReferenceItems.FindAsync(id);
        if (entity == null)
            return NotFound();

        entity.Name = dto.Name;
        entity.LogoMediaId = dto.LogoMediaId;
        entity.Order = dto.Order;
        entity.IsActive = dto.IsActive;

        await _db.SaveChangesAsync();
        return Ok(new { message = "Referans güncellendi." });
    }

    // =====================================
    // HARD DELETE
    // DELETE: /api/references/{id}
    // =====================================
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var entity = await _db.ReferenceItems.FindAsync(id);
        if (entity == null)
            return NotFound();

        _db.ReferenceItems.Remove(entity);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Referans silindi." });
    }
}
