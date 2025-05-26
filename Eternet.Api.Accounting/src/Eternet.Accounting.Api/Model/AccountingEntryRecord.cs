using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public class AccountingEntryRecord
{
    public required int Id { get; set; }
    public required int AccountingEntryDetailId { get; set; }
    public JournalEntryDetail AccountingEntryDetail { get; set; } = null!;
    public required int AccountingReferenceId { get; set; }
    public AccountingReference AccountingReference { get; set; } = null!;
}

public class AccountingEntryRecordConfiguration : IEntityTypeConfiguration<AccountingEntryRecord>
{
    public void Configure(EntityTypeBuilder<AccountingEntryRecord> builder)
    {
        builder.ToTable("CONTAB_DIARIO_DET_CPBTES");
        builder.Property(x => x.Id).HasColumnName("ID");
        builder.Property(x => x.AccountingEntryDetailId).HasColumnName("ID_CONTAB_DIARIO_DET");
        builder.Property(x => x.AccountingReferenceId).HasColumnName("ID_CONTAB_TABLA_CAMPO");

        builder.HasOne(x => x.AccountingEntryDetail)
            .WithMany()
            .HasForeignKey(x => x.AccountingEntryDetailId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.AccountingReference)
            .WithMany()
            .HasForeignKey(x => x.AccountingReferenceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}