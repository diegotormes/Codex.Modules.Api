using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public class AccountingPeriod
{
    public int Id { get; set; }
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    public required string Description { get; set; }
    public DateOnly? CloseDate { get; set; }
    public DateOnly? PartialCloseDate { get; set; }
    public string? PartialCloseUser { get; set; }
}

public class AccountingPeriodConfiguration : IEntityTypeConfiguration<AccountingPeriod>
{
    public void Configure(EntityTypeBuilder<AccountingPeriod> builder)
    {
        builder.ToTable("CONTAB_EJERCICIOS");
        builder.Property(x => x.Id).HasColumnName("ID").ValueGeneratedOnAdd();
        builder.Property(x => x.StartDate).HasColumnName("FECHA_INICIO");
        builder.Property(x => x.EndDate).HasColumnName("FECHA_FIN");
        builder.Property(x => x.Description).HasColumnName("DESCRIPCION");
        builder.Property(x => x.CloseDate).HasColumnName("FECHA_CIERRE");
        builder.Property(x => x.PartialCloseDate).HasColumnName("FECHA_CIERRE_PARCIAL");
        builder.Property(x => x.PartialCloseUser).HasColumnName("USER_CIERRE_PARCIAL");

        builder.HasMany<GeneralLedgerAccount>()
            .WithOne(x => x.AccountingPeriod)
            .HasForeignKey(x => x.AccountingPeriodId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<GeneralLedgerAccount>()
            .WithOne(x => x.AccountingPeriod)
            .HasForeignKey(x => x.AccountingPeriodId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}