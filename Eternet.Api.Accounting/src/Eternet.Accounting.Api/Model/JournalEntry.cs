using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Eternet.Models.Abstractions;

namespace Eternet.Accounting.Api.Model;

public class JournalEntry : IIntIdentity
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public int? AccountingPeriodId { get; set; }
    public AccountingPeriod? AccountingPeriod { get; set; }
    public required DateOnly Date { get; set; }
    public required string Description { get; set; }
    public short? Month { get; set; }
    public short? Year { get; set; }
    public short EntryClose { get; set; } // default is 0, so no need to initialize

    public ICollection<JournalEntryDetail> AccountingEntryDetails { get; set; } = [];

}

public class AccountingEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
{
    public void Configure(EntityTypeBuilder<JournalEntry> builder)
    {
        builder.ToTable("CONTAB_DIARIO");
        builder.Property(x => x.Id).HasColumnName("ID").ValueGeneratedOnAdd();
        builder.Property(x => x.Guid).HasColumnName("GUID").HasColumnType("CHAR(16) CHARACTER SET OCTETS");
        builder.Property(x => x.AccountingPeriodId).HasColumnName("ID_EJERCICIO");
            
        builder.Property(x => x.Date).HasColumnName("FECHA");
        builder.Property(x => x.Description).HasColumnName("DESCRIPCION").HasMaxLength(100);
        builder.Property(x => x.Month).HasColumnName("MES");
        builder.Property(x => x.Year).HasColumnName("ANIO");
        builder.Property(x => x.EntryClose).HasColumnName("ASIENTO_CIERRE");

        builder.HasOne(x => x.AccountingPeriod)
            .WithMany()
            .HasForeignKey(x => x.AccountingPeriodId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.AccountingEntryDetails)
               .WithOne(x => x.JournalEntry)
               .HasForeignKey(x => x.JournalEntryId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
