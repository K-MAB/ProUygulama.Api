using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProUygulama.Api.Data;
using ProUygulama.Api.Dtos;
using ProUygulama.Api.Entities;

namespace ProUygulama.Api.Controllers;

[ApiController]
[Route("api/services")]
public class ServicesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ServicesController(AppDbContext db)
    {
        _db = db;
    }

    // =====================================
    // FRONTEND
    // GET: /api/services
    // =====================================
    [HttpGet]
    public async Task<IActionResult> GetActive()
    {
        var list = await _db.ServiceItems
            .Where(x => x.IsActive)
            .OrderBy(x => x.Order)
            .Select(x => new ServiceResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                ContentHtml = x.ContentHtml,
                Icon = x.Icon,
                Order = x.Order
            })
            .ToListAsync();

        return Ok(list);
    }

    // =====================================
    // ADMIN LIST
    // GET: /api/services/admin
    // =====================================
    [HttpGet("admin")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _db.ServiceItems
            .OrderBy(x => x.Order)
            .ToListAsync());
    }

    // =====================================
    // CREATE
    // POST: /api/services
    // =====================================
    [HttpPost]
    public async Task<IActionResult> Create(CreateServiceDto dto)
    {
        var entity = new ServiceItem
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            ContentHtml = dto.ContentHtml,
            Icon = dto.Icon,
            Order = dto.Order,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _db.ServiceItems.Add(entity);
        await _db.SaveChangesAsync();

        return Ok(entity);
    }

    // =====================================
    // UPDATE
    // PUT: /api/services/{id}
    // =====================================
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateServiceDto dto)
    {
        var entity = await _db.ServiceItems.FindAsync(id);
        if (entity == null)
            return NotFound();

        entity.Title = dto.Title;
        entity.ContentHtml = dto.ContentHtml;
        entity.Icon = dto.Icon;
        entity.Order = dto.Order;
        entity.IsActive = dto.IsActive;

        await _db.SaveChangesAsync();
        return Ok(new { message = "Hizmet güncellendi." });
    }

    // =====================================
    // HARD DELETE
    // DELETE: /api/services/{id}
    // =====================================
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var entity = await _db.ServiceItems.FindAsync(id);
        if (entity == null)
            return NotFound();

        _db.ServiceItems.Remove(entity);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Hizmet silindi." });
    }
}
