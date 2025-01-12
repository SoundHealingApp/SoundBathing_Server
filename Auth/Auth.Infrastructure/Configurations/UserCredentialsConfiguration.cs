using Auth.Infrastructure.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Configurations;

public class UserCredentialsConfiguration : IEntityTypeConfiguration<UserCredentialsEntity>
{
    public void Configure(EntityTypeBuilder<UserCredentialsEntity> builder)
    {
        builder.ToTable("UserCredentials");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email).IsRequired();

        builder.Property(x => x.PasswordHash).IsRequired();
    }
}