using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProUygulama.Api.Data;
using ProUygulama.Api.Dtos;
using ProUygulama.Api.Entities;

namespace ProUygulama.Api.Controllers;

[ApiController]
[Route("api/mission-vision")]
public class MissionVisionController : ControllerBase
{
    private readonly AppDbContext _db;

    public MissionVisionController(AppDbContext db)
    {
        _db = db;
    }

    // =====================================
    // FRONTEND (AKTİF KAYITLAR)
    // GET: /api/mission-vision
    // =====================================
    [HttpGet]
    public async Task<IActionResult> GetActive()
    {
        var list = await _db.MissionVisions
            .Where(x => x.IsActive)
            .OrderBy(x => x.Order)
            .Select(x => new MissionVisionResponseDto
            {
                Id = x.Id,
                Type = x.Type,
                Title = x.Title,
                Description = x.Description,
                Icon = x.Icon,
                Order = x.Order
            })
            .ToListAsync();

        return Ok(list);
    }

    // =====================================
    // ADMIN LIST
    // GET: /api/mission-vision/admin
    // =====================================
    [HttpGet("admin")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _db.MissionVisions
            .OrderBy(x => x.Order)
            .ToListAsync());
    }

    // =====================================
    // CREATE
    // POST: /api/mission-vision
    // =====================================
    [HttpPost]
    public async Task<IActionResult> Create(CreateMissionVisionDto dto)
    {
        var entity = new MissionVision
        {
            Id = Guid.NewGuid(),
            Type = dto.Type,
            Title = dto.Title,
            Description = dto.Description,
            Icon = dto.Icon,
            Order = dto.Order,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _db.MissionVisions.Add(entity);
        await _db.SaveChangesAsync();

        return Ok(entity);
    }

    // =====================================
    // UPDATE
    // PUT: /api/mission-vision/{id}
    // =====================================
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateMissionVisionDto dto)
    {
        var entity = await _db.MissionVisions.FindAsync(id);
        if (entity == null)
            return NotFound();

        entity.Title = dto.Title;
        entity.Description = dto.Description;
        entity.Icon = dto.Icon;
        entity.Order = dto.Order;
        entity.IsActive = dto.IsActive;

        await _db.SaveChangesAsync();
        return Ok(new { message = "Güncellendi" });
    }

    // =====================================
    // HARD DELETE
    // DELETE: /api/mission-vision/{id}
    // =====================================
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var entity = await _db.MissionVisions.FindAsync(id);
        if (entity == null)
            return NotFound();

        _db.MissionVisions.Remove(entity);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Silindi" });
    }
}
