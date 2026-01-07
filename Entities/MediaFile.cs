namespace ProUygulama.Api.Entities;

public class MediaFile
{
    public Guid Id { get; set; }

    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Size { get; set; }

    public string Path { get; set; } = null!;

    // Dışarıya açık URL
    public string PublicUrl { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
