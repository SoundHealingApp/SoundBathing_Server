using Microsoft.EntityFrameworkCore;
using SoundHealing.Core.Models;
using SoundHealing.Infrastructure.Configurations;

namespace SoundHealing.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<UserCredentials> UsersCredentials { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Meditation> Meditations { get; set; }
    public DbSet<MeditationFeedback> MeditationsFeedback { get; set; }
    public DbSet<LiveStream> LiveStreams { get; set; }
    public DbSet<Quote> Quotes { get; set; }
    
    public DbSet<Permission> Permission { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserCredentialsConfiguration());
        modelBuilder.ApplyConfiguration(new MeditationConfiguration());
        modelBuilder.ApplyConfiguration(new MeditationFeedbackConfiguration());
        modelBuilder.ApplyConfiguration(new LiveStreamConfiguration());
        modelBuilder.ApplyConfiguration(new QuoteConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}