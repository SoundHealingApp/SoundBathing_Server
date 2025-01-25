using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Configurations;

public class MeditationConfiguration : IEntityTypeConfiguration<Meditation>
{
    public void Configure(EntityTypeBuilder<Meditation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired();
        
        builder.Property(x => x.Description).IsRequired();
        
        builder.Property(x => x.MeditationType).IsRequired();
        
        builder.Property(x => x.TherapeuticPurpose);

        builder.Property(x => x.Rating);
        
        builder.Property(x => x.ImageLink);
        
        builder.Property(x => x.AudioLink);

        builder.Property(x => x.Frequency);

        builder
            .HasMany(x => x.Feedbacks)
            .WithOne()
            .HasForeignKey(x => x.MeditationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}