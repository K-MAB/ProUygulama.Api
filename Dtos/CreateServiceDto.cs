public class CreateServiceDto
{
    public string Title { get; set; } = null!;
    public string ContentHtml { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public int Order { get; set; }
}
