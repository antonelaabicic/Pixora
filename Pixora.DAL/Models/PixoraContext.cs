using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Pixora.DAL.Models;

public class PixoraContext : IdentityDbContext<ApplicationUser>
{
    public PixoraContext(DbContextOptions<PixoraContext> options)
        : base(options)
    {
    }

    public DbSet<Photo> Photos => Set<Photo>();

    public DbSet<Hashtag> Hashtags => Set<Hashtag>();

    public DbSet<PhotoHashtag> PhotoHashtags => Set<PhotoHashtag>();

    public DbSet<UserActionLog> UserActionLogs => Set<UserActionLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .Property(u => u.PlanType)
            .HasConversion<string>();

        builder.Entity<ApplicationUser>()
            .Property(u => u.PendingPlanType)
            .HasConversion<string>();

        builder.Entity<UserActionLog>()
            .Property(l => l.ActionType)
            .HasConversion<string>();

        builder.Entity<PhotoHashtag>()
            .HasKey(ph => new { ph.PhotoId, ph.HashtagId });

        builder.Entity<PhotoHashtag>()
            .HasOne(ph => ph.Photo)
            .WithMany(p => p.PhotoHashtags)
            .HasForeignKey(ph => ph.PhotoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PhotoHashtag>()
            .HasOne(ph => ph.Hashtag)
            .WithMany(h => h.PhotoHashtags)
            .HasForeignKey(ph => ph.HashtagId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Hashtag>()
            .HasIndex(h => h.Name)
            .IsUnique();

        builder.Entity<Photo>()
            .HasOne(p => p.Author)
            .WithMany(u => u.Photos)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserActionLog>()
            .HasOne(l => l.User)
            .WithMany(u => u.ActionLogs)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}