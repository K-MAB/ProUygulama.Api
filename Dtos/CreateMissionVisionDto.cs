public class CreateMissionVisionDto
{
    public string Type { get; set; } = null!; // Mission | Vision
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public int Order { get; set; }
}
