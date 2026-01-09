namespace ProUygulama.Api.Dtos;

public class ProjectListDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;

    public string City { get; set; } = null!;
    public string District { get; set; } = null!;

    public string? CoverImageUrl { get; set; }
}
