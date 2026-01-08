using Microsoft.EntityFrameworkCore;
using ProUygulama.Api.Entities;

namespace ProUygulama.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<HeaderContent> HeaderContents => Set<HeaderContent>();
    public DbSet<MediaFile> MediaFiles => Set<MediaFile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Header -> Background Video ilişki
        modelBuilder.Entity<HeaderContent>()
            .HasOne(x => x.BackgroundVideo)
            .WithMany()
            .HasForeignKey(x => x.BackgroundVideoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
