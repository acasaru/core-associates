using Interview.Application.Core.Entitities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Interview.Infrastructure.Persistence.Configurations
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.ToTable(InfrastructureConstants.TableNames.Note);

            builder.HasKey(e => e.NoteId);

            builder.Property(e => e.NoteId)
                .IsRequired(true);

            builder.Property(e => e.Text)
                .HasMaxLength(2048)
                .IsRequired(true);

            builder.HasOne(y => y.Invoice)
              .WithMany(x => x.Notes)
              .HasForeignKey(x => x.InvoiceId);
        }
    }
}
