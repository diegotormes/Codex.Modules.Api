using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public class AccountingReference
{
    public required int Id { get; set; }
    public required string Table { get; set; }
    public required string Field { get; set; }
}

public class AccountingReferenceConfiguration : IEntityTypeConfiguration<AccountingReference>
{
    public void Configure(EntityTypeBuilder<AccountingReference> builder)
    {
        builder.ToTable("CONTAB_TABLA_Y_CAMPOS");
        builder.Property(x => x.Id).HasColumnName("ID");
        builder.Property(x => x.Table).HasColumnName("TABLA");
        builder.Property(x => x.Field).HasColumnName("CAMPOS");

        builder.HasMany<AccountingEntryRecord>()
            .WithOne(x => x.AccountingReference)
            .HasForeignKey(x => x.AccountingReferenceId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}