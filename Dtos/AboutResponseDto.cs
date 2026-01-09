namespace ProUygulama.Api.Dtos;

public class AboutResponseDto
{
    public Guid Id { get; set; }
    public string ContentHtml { get; set; } = null!;
    public string? Title { get; set; }
}
