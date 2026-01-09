using System;

namespace ProUygulama.Api.Entities;

public class ServiceItem
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    // 🔥 RICH TEXT (HTML)
    public string ContentHtml { get; set; } = null!;

    // ikon adı veya media key
    public string Icon { get; set; } = null!;

    public int Order { get; set; }
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}
