public class UpdateReferenceDto
{
    public string Name { get; set; } = null!;
    public Guid? LogoMediaId { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; }
}
