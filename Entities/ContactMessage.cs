namespace ProUygulama.Api.Entities;

public class ContactMessage
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }

    public string Subject { get; set; } = null!;
    public string Message { get; set; } = null!;

    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
