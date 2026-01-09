namespace ProUygulama.Api.Dtos;

public class ProjectDetailDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string DescriptionHtml { get; set; } = null!;

    public string City { get; set; } = null!;
    public string District { get; set; } = null!;

    public List<string> Categories { get; set; } = new();
    public List<string> GalleryUrls { get; set; } = new();
}
