using Microsoft.EntityFrameworkCore;
using SoundHealing.Core.Models;
using SoundHealing.Infrastructure.Configurations;

namespace SoundHealing.Infrastructure;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<UserCredentials> UsersCredentials { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Meditation> Meditations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserCredentialsConfiguration());
        modelBuilder.ApplyConfiguration(new MeditationConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}