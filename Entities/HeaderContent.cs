using System.ComponentModel.DataAnnotations.Schema;

namespace ProUygulama.Api.Entities;

public class HeaderContent
{
    public Guid Id { get; set; }

    public string CompanyName { get; set; } = null!;
    public string Slogan { get; set; } = null!;

    public string PrimaryButtonText { get; set; } = null!;
    public string PrimaryButtonUrl { get; set; } = null!;

    public string SecondaryButtonText { get; set; } = null!;
    public string SecondaryButtonUrl { get; set; } = null!;

    public Guid? BackgroundVideoId { get; set; }
    public MediaFile? BackgroundVideo { get; set; }

    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

