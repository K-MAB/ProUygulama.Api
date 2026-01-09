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
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    // =========================
    // MEDIA & PROJECTS
    // =========================
    public DbSet<MediaFile> MediaFiles => Set<MediaFile>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ProjectCategory> ProjectCategories => Set<ProjectCategory>();
    public DbSet<ProjectGalleryItem> ProjectGalleryItems => Set<ProjectGalleryItem>();

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

        // =========================
        // PROJECT <-> CATEGORY (MANY TO MANY)
        // =========================
        modelBuilder.Entity<ProjectCategory>()
            .HasKey(x => new { x.ProjectId, x.CategoryId });

        modelBuilder.Entity<ProjectCategory>()
            .HasOne(x => x.Project)
            .WithMany(x => x.Categories)
            .HasForeignKey(x => x.ProjectId);

        modelBuilder.Entity<ProjectCategory>()
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId);

        // =========================
        // PROJECT -> COVER MEDIA
        // =========================
        modelBuilder.Entity<Project>()
            .HasOne(x => x.CoverMedia)
            .WithMany()
            .HasForeignKey(x => x.CoverMediaId)
            .OnDelete(DeleteBehavior.SetNull);

        // =========================
        // PROJECT -> GALLERY
        // =========================
        modelBuilder.Entity<ProjectGalleryItem>()
            .HasOne(x => x.Project)
            .WithMany(x => x.Gallery)
            .HasForeignKey(x => x.ProjectId);

        modelBuilder.Entity<ProjectGalleryItem>()
            .HasOne(x => x.MediaFile)
            .WithMany()
            .HasForeignKey(x => x.MediaFileId)
            .OnDelete(DeleteBehavior.Cascade);

        // =========================
        // CATEGORY SLUG UNIQUE (SEO)
        // =========================
        modelBuilder.Entity<Category>()
            .HasIndex(x => x.Slug)
            .IsUnique();
    }
}
