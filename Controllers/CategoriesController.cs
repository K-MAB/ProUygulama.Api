using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProUygulama.Api.Data;
using ProUygulama.Api.Dtos;
using ProUygulama.Api.Entities;
using ProUygulama.Api.Helpers;

namespace ProUygulama.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _db;

    public CategoriesController(AppDbContext db)
    {
        _db = db;
    }

    // ============================
    // FRONTEND (SADECE AKTİF)
    // GET: /api/categories
    // ============================
    [HttpGet]
    public async Task<IActionResult> GetActive()
    {
        var list = await _db.Categories
            .Where(x => x.IsActive)
            .OrderBy(x => x.Order)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Slug
            })
            .ToListAsync();

        return Ok(list);
    }

    // ============================
    // ADMIN (TÜMÜ)
    // GET: /api/categories/admin
    // ============================
    [HttpGet("admin")]
    public async Task<IActionResult> GetAll()
    {
        var list = await _db.Categories
            .OrderBy(x => x.Order)
            .ToListAsync();

        return Ok(list);
    }

    // ============================
    // CREATE
    // POST: /api/categories
    // ============================
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        var slug = SlugHelper.Generate(dto.Name);

        var exists = await _db.Categories.AnyAsync(x => x.Slug == slug);
        if (exists)
            return BadRequest("Bu kategori zaten mevcut.");

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Slug = slug,
            Order = dto.Order,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _db.Categories.Add(category);
        await _db.SaveChangesAsync();

        return Ok(category);
    }

    // ============================
    // UPDATE
    // PUT: /api/categories/{id}
    // ============================
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateCategoryDto dto)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category == null)
            return NotFound();

        category.Name = dto.Name;
        category.Slug = SlugHelper.Generate(dto.Name);
        category.Order = dto.Order;
        category.IsActive = dto.IsActive;

        await _db.SaveChangesAsync();
        return Ok(new { message = "Kategori güncellendi." });
    }

    // ============================
    // HARD DELETE
    // DELETE: /api/categories/{id}
    // ============================
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category == null)
            return NotFound();

        // Projelerde kullanılıyor mu?
        var used = await _db.ProjectCategories
            .AnyAsync(x => x.CategoryId == id);

        if (used)
            return BadRequest("Bu kategori projelerde kullanılıyor.");

        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Kategori silindi." });
    }
}
