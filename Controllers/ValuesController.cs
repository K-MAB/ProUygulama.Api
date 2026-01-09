using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProUygulama.Api.Data;
using ProUygulama.Api.Dtos;
using ProUygulama.Api.Entities;

namespace ProUygulama.Api.Controllers;

[ApiController]
[Route("api/values")]
public class ValuesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ValuesController(AppDbContext db)
    {
        _db = db;
    }

    // =====================================
    // FRONTEND
    // GET: /api/values
    // =====================================
    [HttpGet]
    public async Task<IActionResult> GetActive()
    {
        var list = await _db.ValueItems
            .Where(x => x.IsActive)
            .OrderBy(x => x.Order)
            .Select(x => new ValueResponseDto
            {
                Id = x.Id,
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
    // GET: /api/values/admin
    // =====================================
    [HttpGet("admin")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _db.ValueItems
            .OrderBy(x => x.Order)
            .ToListAsync());
    }

    // =====================================
    // CREATE
    // POST: /api/values
    // =====================================
    [HttpPost]
    public async Task<IActionResult> Create(CreateValueDto dto)
    {
        var entity = new ValueItem
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Icon = dto.Icon,
            Order = dto.Order,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _db.ValueItems.Add(entity);
        await _db.SaveChangesAsync();

        return Ok(entity);
    }

    // =====================================
    // UPDATE
    // PUT: /api/values/{id}
    // =====================================
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateValueDto dto)
    {
        var entity = await _db.ValueItems.FindAsync(id);
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
    // DELETE: /api/values/{id}
    // =====================================
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var entity = await _db.ValueItems.FindAsync(id);
        if (entity == null)
            return NotFound();

        _db.ValueItems.Remove(entity);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Silindi" });
    }
}
