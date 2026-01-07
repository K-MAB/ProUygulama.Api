using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProUygulama.Api.Data;
using ProUygulama.Api.Entities;

namespace ProUygulama.Api.Controllers;

[ApiController]
[Route("api/media-files")]
public class MediaFilesController : ControllerBase
{
    private readonly AppDbContext _db;

    public MediaFilesController(AppDbContext db)
    {
        _db = db;
    }

    // GET: /api/media-files
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _db.MediaFiles
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return Ok(list);
    }

    // POST: /api/media-files
    // TEST amaçlı (dosya upload yok)
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var media = new MediaFile
        {
            Id = Guid.NewGuid(),
            FileName = "test-video.mp4",
            ContentType = "video/mp4",
            Size = 123456,
            Path = "/uploads/test-video.mp4",
            PublicUrl = "https://localhost/uploads/test-video.mp4",
            CreatedAt = DateTime.UtcNow
        };

        _db.MediaFiles.Add(media);
        await _db.SaveChangesAsync();

        return Ok(media);
    }
}
