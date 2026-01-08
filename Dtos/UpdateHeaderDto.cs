namespace ProUygulama.Api.Dtos;

public class UpdateHeaderDto
{
    public string CompanyName { get; set; } = null!;
    public string Slogan { get; set; } = null!;

    public string PrimaryButtonText { get; set; } = null!;
    public string PrimaryButtonUrl { get; set; } = null!;

    public string SecondaryButtonText { get; set; } = null!;
    public string SecondaryButtonUrl { get; set; } = null!;
}
