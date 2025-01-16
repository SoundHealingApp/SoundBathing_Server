using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Surname)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.BirthDate).IsRequired();
        
        builder.HasOne<UserCredentials>()
            .WithOne()
            .HasForeignKey<UserCredentials>(x => x.Id);
    
        builder
            .HasMany(x => x.LikedMeditations)
            .WithMany(x => x.LikedUsers);
    }
}