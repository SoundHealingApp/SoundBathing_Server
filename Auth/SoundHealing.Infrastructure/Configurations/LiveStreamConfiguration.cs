using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Configurations;

public class LiveStreamConfiguration : IEntityTypeConfiguration<LiveStream>
{
    public void Configure(EntityTypeBuilder<LiveStream> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.TherapeuticPurpose);
        
        builder.Property(x => x.StartDateTime).IsRequired();
        builder.Property(x => x.YouTubeUrl).IsRequired();
    }
}