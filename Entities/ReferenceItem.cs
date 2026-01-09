using System;

namespace ProUygulama.Api.Entities;

public class ReferenceItem
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    // Logo Media
    public Guid? LogoMediaId { get; set; }
    public MediaFile? LogoMedia { get; set; }

    public int Order { get; set; }
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}
