using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Configurations;

public class MeditationFeedbackConfiguration : IEntityTypeConfiguration<MeditationFeedback>
{
    public void Configure(EntityTypeBuilder<MeditationFeedback> builder)
    {
        builder.HasKey(x => new { x.UserId, x.MeditationId });
        
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.MeditationId).IsRequired();
        builder.Property(x => x.UserName).IsRequired();

        builder.Property(x => x.Comment);
        builder.Property(x => x.Estimate).IsRequired();
    }
}