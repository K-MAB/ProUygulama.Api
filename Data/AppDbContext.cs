using Microsoft.EntityFrameworkCore;
using ProUygulama.Api.Entities;

namespace ProUygulama.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<MediaFile> MediaFiles => Set<MediaFile>();
}
