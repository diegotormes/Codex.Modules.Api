using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public class AccountingEntryRecordDetail
{
    public required int Id { get; set; }
    public required int AccountingEntryRecordId { get; set; }
    public AccountingEntryRecord AccountingEntryRecord { get; set; } = null!;
    public required int TableId { get; set; }
    public required DateOnly Date { get; set; }
    public string? Detail { get; set; }
    public double? Debit { get; set; }
    public double? Credit { get; set; }
}

public class AccountingEntryRecordDetailConfiguration : IEntityTypeConfiguration<AccountingEntryRecordDetail>
{
    public void Configure(EntityTypeBuilder<AccountingEntryRecordDetail> builder)
    {
        builder.ToTable("CONTAB_DIARIO_DET_CPBTES_IDS");
        builder.Property(x => x.Id).HasColumnName("ID");
        builder.Property(x => x.AccountingEntryRecordId).HasColumnName("ID_CONTAB_DIARIO_CPBTE");
        builder.Property(x => x.TableId).HasColumnName("ID_TABLA");
        builder.Property(x => x.Date).HasColumnName("FECHA");
        builder.Property(x => x.Detail).HasColumnName("DETALLE");
        builder.Property(x => x.Debit).HasColumnName("DEBE");
        builder.Property(x => x.Credit).HasColumnName("HABER");

        builder.HasOne(x => x.AccountingEntryRecord)
            .WithMany()
            .HasForeignKey(x => x.AccountingEntryRecordId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}