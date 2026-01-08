using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProUygulama.Api.Data;
using ProUygulama.Api.Dtos;
using ProUygulama.Api.Entities;

[ApiController]
[Route("api/media-files")]
public class MediaFilesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    private const long MAX_VIDEO_SIZE = 200 * 1024 * 1024; // 200 MB
    private const long MAX_IMAGE_SIZE = 10 * 1024 * 1024;  // 10 MB

    private static readonly string[] IMAGE_TYPES =
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };

    private static readonly string[] VIDEO_TYPES =
    {
        "video/mp4",
        "video/webm"
    };

    public MediaFilesController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    // ======================================================
    // UPLOAD
    // ======================================================
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(MAX_VIDEO_SIZE)]
    [RequestFormLimits(MultipartBodyLengthLimit = MAX_VIDEO_SIZE)]
    public async Task<IActionResult> Upload([FromForm] MediaFileUploadDto dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("Dosya bulunamadı.");

        var file = dto.File;

        var isImage = IMAGE_TYPES.Contains(file.ContentType);
        var isVideo = VIDEO_TYPES.Contains(file.ContentType);

        if (!isImage && !isVideo)
            return BadRequest("Sadece resim veya video yüklenebilir.");

        if (isImage && file.Length > MAX_IMAGE_SIZE)
            return BadRequest("Resim max 10 MB olabilir.");

        if (isVideo && file.Length > MAX_VIDEO_SIZE)
            return BadRequest("Video max 200 MB olabilir.");

        var uploadRoot = Path.Combine(
            _env.WebRootPath ?? "wwwroot",
            "uploads"
        );

        Directory.CreateDirectory(uploadRoot);

        var ext = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{ext}";
        var fullPath = Path.Combine(uploadRoot, fileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        var media = new MediaFile
        {
            Id = Guid.NewGuid(),
            FileName = file.FileName,
            ContentType = file.ContentType,
            Size = file.Length,
            Path = $"/uploads/{fileName}",
            PublicUrl = $"/uploads/{fileName}",
            MediaType = isVideo ? MediaType.Video : MediaType.Image,
            CreatedAt = DateTime.UtcNow
        };

        _db.MediaFiles.Add(media);
        await _db.SaveChangesAsync();

        return Ok(media);
    }

    // ======================================================
    // LIST (ADMIN)
    // ======================================================
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _db.MediaFiles
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return Ok(list);
    }

    // ======================================================
    // DELETE
    // ======================================================
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var media = await _db.MediaFiles.FindAsync(id);
        if (media == null)
            return NotFound();

        // 🔥 Header'dan kopar
        var headersUsingMedia = await _db.HeaderContents
            .Where(x => x.BackgroundVideoId == id)
            .ToListAsync();

        foreach (var header in headersUsingMedia)
            header.BackgroundVideoId = null;

        // Diskten sil
        var physicalPath = Path.Combine(
            _env.WebRootPath ?? "wwwroot",
            media.Path.TrimStart('/')
        );

        if (System.IO.File.Exists(physicalPath))
            System.IO.File.Delete(physicalPath);

        _db.MediaFiles.Remove(media);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Media silindi ve header'dan kaldırıldı." });
    }
}
