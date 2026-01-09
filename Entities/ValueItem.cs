using System;
using System.ComponentModel.DataAnnotations;

namespace ProUygulama.Api.Entities;

public class ValueItem
{
    [Key]
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;

    // ikon adı (frontend karar verir: lucide, heroicons vs.)
    public string Icon { get; set; } = null!;

    public int Order { get; set; }
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}
