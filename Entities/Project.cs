namespace ProUygulama.Api.Entities;

public class Project
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;

    public string DescriptionHtml { get; set; } = null!; // RichText

    public string City { get; set; } = null!;
    public string District { get; set; } = null!;

    public Guid? CoverMediaId { get; set; }
    public MediaFile? CoverMedia { get; set; }

    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    // Relations
    public ICollection<ProjectCategory> Categories { get; set; } = new List<ProjectCategory>();
    public ICollection<ProjectGalleryItem> Gallery { get; set; } = new List<ProjectGalleryItem>();
}
