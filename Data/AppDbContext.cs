
using Microsoft.EntityFrameworkCore;
using SkillSwapBackend.Models;

namespace SkillSwapBackend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    // Existing table
    public DbSet<Skill> Skills { get; set; }

    // Skills the user already knows
    public DbSet<UserSkill> UserSkills { get; set; }

    // Skills the user wants to learn
    public DbSet<LearningSkill> LearningSkills { get; set; }

    public DbSet<SwapRequest> SwapRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SwapRequest>()
            .HasOne(request => request.Sender)
            .WithMany()
            .HasForeignKey(request => request.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SwapRequest>()
            .HasOne(request => request.Receiver)
            .WithMany()
            .HasForeignKey(request => request.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
