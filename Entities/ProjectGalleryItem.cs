namespace ProUygulama.Api.Entities;

public class ProjectGalleryItem
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public Guid MediaFileId { get; set; }
    public MediaFile MediaFile { get; set; } = null!;

    public int Order { get; set; }
}
