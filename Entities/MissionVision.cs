using System;

namespace ProUygulama.Api.Entities;

public class MissionVision
{
    public Guid Id { get; set; }

    // "Mission" | "Vision"
    public string Type { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;

    // ikon adı (lucide / heroicons / vs.)
    public string Icon { get; set; } = null!;

    public int Order { get; set; }
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}
