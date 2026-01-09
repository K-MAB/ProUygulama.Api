using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProUygulama.Api.Data;
using ProUygulama.Api.Dtos;
using ProUygulama.Api.Entities;
using ProUygulama.Api.Helpers;

namespace ProUygulama.Api.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProjectsController(AppDbContext db)
    {
        _db = db;
    }

    // =========================
    // FRONT LIST
    // GET /api/projects
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] Guid? categoryId)
    {
        var query = _db.Projects
            .Include(x => x.CoverMedia)
            .Include(x => x.Categories).ThenInclude(x => x.Category)
            .Where(x => x.IsActive);

        if (categoryId.HasValue)
            query = query.Where(x => x.Categories.Any(c => c.CategoryId == categoryId));

        var list = await query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ProjectListDto
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                City = x.City,
                District = x.District,
                CoverImageUrl = x.CoverMedia!.PublicUrl
            })
            .ToListAsync();

        return Ok(list);
    }

    // =========================
    // FRONT DETAIL
    // GET /api/projects/{slug}
    // =========================
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetDetail(string slug)
    {
        var project = await _db.Projects
            .Include(x => x.Categories).ThenInclude(x => x.Category)
            .Include(x => x.Gallery).ThenInclude(x => x.MediaFile)
            .FirstOrDefaultAsync(x => x.Slug == slug && x.IsActive);

        if (project == null)
            return NotFound();

        return Ok(new ProjectDetailDto
        {
            Id = project.Id,
            Title = project.Title,
            DescriptionHtml = project.DescriptionHtml,
            City = project.City,
            District = project.District,
            Categories = project.Categories.Select(x => x.Category.Name).ToList(),
            GalleryUrls = project.Gallery
                .OrderBy(x => x.Order)
                .Select(x => x.MediaFile.PublicUrl)
                .ToList()
        });
    }

    // =========================
    // ADMIN CREATE
    // POST /api/projects
    // =========================
    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectDto dto)
    {
        var slug = SlugHelper.Generate(dto.Title);

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Slug = slug,
            DescriptionHtml = dto.DescriptionHtml,
            City = dto.City,
            District = dto.District,
            CoverMediaId = dto.CoverMediaId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var catId in dto.CategoryIds)
        {
            project.Categories.Add(new ProjectCategory
            {
                ProjectId = project.Id,
                CategoryId = catId
            });
        }

        _db.Projects.Add(project);
        await _db.SaveChangesAsync();

        return Ok(project);
    }

    // =========================
    // ADMIN UPDATE
    // PUT /api/projects/{id}
    // =========================
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateProjectDto dto)
    {
        var project = await _db.Projects
            .Include(x => x.Categories)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (project == null)
            return NotFound();

        project.Title = dto.Title;
        project.Slug = SlugHelper.Generate(dto.Title);
        project.DescriptionHtml = dto.DescriptionHtml;
        project.City = dto.City;
        project.District = dto.District;
        project.CoverMediaId = dto.CoverMediaId;
        project.IsActive = dto.IsActive;

        project.Categories.Clear();
        foreach (var catId in dto.CategoryIds)
        {
            project.Categories.Add(new ProjectCategory
            {
                ProjectId = project.Id,
                CategoryId = catId
            });
        }

        await _db.SaveChangesAsync();
        return Ok(new { message = "Proje güncellendi." });
    }

    // =========================
    // HARD DELETE
    // DELETE /api/projects/{id}
    // =========================
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var project = await _db.Projects.FindAsync(id);
        if (project == null)
            return NotFound();

        _db.Projects.Remove(project);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Proje silindi." });
    }
}
