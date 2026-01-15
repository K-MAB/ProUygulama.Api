namespace ProUygulama.Api.Entities;

public class AdminUser
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;

    // hash (şifre asla plain tutulmaz)
    public string PasswordHash { get; set; } = null!;

    public AdminRole Role { get; set; }

    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
