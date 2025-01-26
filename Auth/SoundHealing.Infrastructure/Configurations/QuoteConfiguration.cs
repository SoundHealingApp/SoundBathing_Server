using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoundHealing.Core.Models;

namespace SoundHealing.Infrastructure.Configurations;

public class QuoteConfiguration : IEntityTypeConfiguration<Quote>
{
    public void Configure(EntityTypeBuilder<Quote> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Text).IsRequired();
        builder.Property(x => x.Author).IsRequired();
    }
}