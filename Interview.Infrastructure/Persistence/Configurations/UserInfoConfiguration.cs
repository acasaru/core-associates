using Interview.Application.Core.Entitities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Interview.Infrastructure.Persistence.Configurations
{
    public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable(InfrastructureConstants.TableNames.UserInfo);

            builder.HasKey(e => e.UserId);

            builder.Property(e => e.UserId)
                .ValueGeneratedNever()
                .IsRequired(true);

            builder.Property(e => e.ApiKey)
                .HasMaxLength(1024)
                .IsRequired(true);

            builder.Property(e => e.Role)
                .HasMaxLength(64)
                .IsRequired(true);

            builder.HasIndex(e => e.ApiKey)
                .IsUnique();

        }
    }
}
