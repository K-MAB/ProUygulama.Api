public class UpdateServiceDto
{
    public string Title { get; set; } = null!;
    public string ContentHtml { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public int Order { get; set; }
    public bool IsActive { get; set; }
}
