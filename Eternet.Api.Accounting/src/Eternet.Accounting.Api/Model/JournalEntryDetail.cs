using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public class JournalEntryDetail
{
    public int Id { get; set; }
    public int JournalEntryId { get; set; }
    public JournalEntry JournalEntry { get; set; } = null!;
    public int GeneralLedgerAccountId { get; set; }
    public GeneralLedgerAccount GeneralLedgerAccount { get; set; } = null!;
    public double? Debit { get; set; }
    public double? Credit { get; set; }
}

public class AccountingEntryDetailConfiguration : IEntityTypeConfiguration<JournalEntryDetail>
{
    public void Configure(EntityTypeBuilder<JournalEntryDetail> builder)
    {
        builder.ToTable("CONTAB_DIARIO_DET");
        builder.Property(x => x.Id).HasColumnName("ID");
        builder.Property(x => x.JournalEntryId).HasColumnName("ID_DIARIO");
        builder.Property(x => x.GeneralLedgerAccountId).HasColumnName("ID_CUENTA");
        builder.Property(x => x.Debit).HasColumnName("DEBE");
        builder.Property(x => x.Credit).HasColumnName("HABER");

        builder.HasOne(x => x.GeneralLedgerAccount)
            .WithMany()
            .HasForeignKey(x => x.GeneralLedgerAccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.GeneralLedgerAccount)
            .WithMany()
            .HasForeignKey(x => x.GeneralLedgerAccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}