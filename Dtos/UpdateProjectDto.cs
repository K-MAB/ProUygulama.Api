namespace ProUygulama.Api.Dtos;

public class UpdateProjectDto
{
    public string Title { get; set; } = null!;
    public string DescriptionHtml { get; set; } = null!;

    public string City { get; set; } = null!;
    public string District { get; set; } = null!;

    public Guid? CoverMediaId { get; set; }
    public bool IsActive { get; set; }

    public List<Guid> CategoryIds { get; set; } = new();
}
