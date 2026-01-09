using Microsoft.EntityFrameworkCore;
using ProUygulama.Api.Entities;

namespace ProUygulama.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // =========================
    // CORE CONTENT
    // =========================
    public DbSet<HeaderContent> HeaderContents => Set<HeaderContent>();
    public DbSet<AboutContent> AboutContents => Set<AboutContent>();
    public DbSet<MissionVision> MissionVisions => Set<MissionVision>();
    public DbSet<ValueItem> ValueItems => Set<ValueItem>();
    public DbSet<ServiceItem> ServiceItems => Set<ServiceItem>();
    public DbSet<ReferenceItem> ReferenceItems => Set<ReferenceItem>();

    // =========================
    // MEDIA
    // =========================
    public DbSet<MediaFile> MediaFiles => Set<MediaFile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================
        // HEADER -> BACKGROUND VIDEO
        // =========================
        modelBuilder.Entity<HeaderContent>()
            .HasOne(x => x.BackgroundVideo)
            .WithMany()
            .HasForeignKey(x => x.BackgroundVideoId)
            .OnDelete(DeleteBehavior.SetNull);

        // =========================
        // REFERENCES -> LOGO MEDIA
        // =========================
        modelBuilder.Entity<ReferenceItem>()
            .HasOne(x => x.LogoMedia)
            .WithMany()
            .HasForeignKey(x => x.LogoMediaId)
            .OnDelete(DeleteBehavior.SetNull);

    
    }
}
