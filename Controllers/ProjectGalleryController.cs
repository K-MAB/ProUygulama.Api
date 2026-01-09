using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProUygulama.Api.Data;
using ProUygulama.Api.Dtos;
using ProUygulama.Api.Entities;

namespace ProUygulama.Api.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/gallery")]
public class ProjectGalleryController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProjectGalleryController(AppDbContext db)
    {
        _db = db;
    }

    // ============================
    // ADD IMAGE TO GALLERY
    // POST /api/projects/{projectId}/gallery
    // ============================
    [HttpPost]
    public async Task<IActionResult> Add(Guid projectId, AddProjectGalleryItemDto dto)
    {
        var projectExists = await _db.Projects.AnyAsync(x => x.Id == projectId);
        if (!projectExists)
            return NotFound("Proje bulunamadı.");

        var mediaExists = await _db.MediaFiles.AnyAsync(x => x.Id == dto.MediaFileId);
        if (!mediaExists)
            return NotFound("MediaFile bulunamadı.");

        var item = new ProjectGalleryItem
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            MediaFileId = dto.MediaFileId,
            Order = dto.Order
        };

        _db.ProjectGalleryItems.Add(item);
        await _db.SaveChangesAsync();

        return Ok(item);
    }

    // ============================
    // UPDATE ORDER (DRAG & DROP)
    // PUT /api/projects/{projectId}/gallery/order
    // ============================
    [HttpPut("order")]
    public async Task<IActionResult> UpdateOrder(
        Guid projectId,
        List<UpdateProjectGalleryOrderDto> items)
    {
        var gallery = await _db.ProjectGalleryItems
            .Where(x => x.ProjectId == projectId)
            .ToListAsync();

        foreach (var item in items)
        {
            var entity = gallery.FirstOrDefault(x => x.Id == item.GalleryItemId);
            if (entity != null)
                entity.Order = item.Order;
        }

        await _db.SaveChangesAsync();
        return Ok(new { message = "Galeri sırası güncellendi." });
    }

    // ============================
    // DELETE ITEM
    // DELETE /api/projects/{projectId}/gallery/{galleryItemId}
    // ============================
    [HttpDelete("{galleryItemId:guid}")]
    public async Task<IActionResult> Delete(Guid projectId, Guid galleryItemId)
    {
        var item = await _db.ProjectGalleryItems
            .FirstOrDefaultAsync(x => x.Id == galleryItemId && x.ProjectId == projectId);

        if (item == null)
            return NotFound();

        _db.ProjectGalleryItems.Remove(item);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Galeri görseli silindi." });
    }
}
