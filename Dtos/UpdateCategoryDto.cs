namespace ProUygulama.Api.Dtos;

public class UpdateCategoryDto
{
    public string Name { get; set; } = null!;
    public int Order { get; set; }
    public bool IsActive { get; set; }
}
