namespace ProUygulama.Api.Entities;

public class AboutContent
{
    public Guid Id { get; set; }

    // Rich Text (HTML)
    public string ContentHtml { get; set; } = null!;

    // Opsiyonel: başlık
    public string? Title { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
