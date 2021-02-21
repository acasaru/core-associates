using Interview.Application.Core.Entitities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Interview.Infrastructure.Persistence.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable(InfrastructureConstants.TableNames.Invoice);

            builder.HasKey(e => e.InvoiceId);

            builder.Property(e => e.InvoiceId)
                .IsRequired(true);

            builder.Property(e => e.Identifier)
                .HasMaxLength(64)
                .IsUnicode(true)
                .IsRequired(true);

            builder.Property(e => e.Amount)
                .IsRequired(true);

            builder.HasIndex(e => e.Identifier)
                .IsUnique();

            builder.HasMany(e => e.Notes).WithOne(e => e.Invoice);
        }
    }
}
