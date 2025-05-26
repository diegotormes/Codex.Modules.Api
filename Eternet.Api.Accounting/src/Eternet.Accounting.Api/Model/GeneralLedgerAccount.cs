using Eternet.Accounting.Api.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public enum NormalBalanceType
{
    Debit,
    Credit
}

public enum LedgerAccountType
{
    Asset,               // PA (Activo)
    LiabilityAndEquity,  // PP (Pasivo y Patrimonio)
    AccumulatedResult,   // RA (Resultado Acumulado)
    NegativeResult,      // RN (Resultado Negativo)
    PositiveResult,      // RP (Resultado Positivo)
    UnassignedResult     // RS (Resultado Sin Asignar)
}

public class GeneralLedgerAccount
{
    public required int Id { get; set; }
    public required short Order { get; set; }
    public required string Description { get; set; }
    public string? Notes { get; set; }
    public int? ParentLedgerAccountId { get; set; }
    public int? AccountingPeriodId { get; set; }
    public AccountingPeriod? AccountingPeriod { get; set; }
    public required NormalBalanceType NormalBalance { get; set; }
    public required LedgerAccountType LedgerAccountType { get; set; }
}

public class AccountingChartConfiguration : IEntityTypeConfiguration<GeneralLedgerAccount>
{
    public void Configure(EntityTypeBuilder<GeneralLedgerAccount> builder)
    {
        builder.ToTable("CONTAB_PLAN_CUENTAS");
        builder.Property(x => x.Id).HasColumnName("ID");
        builder.Property(x => x.Order).HasColumnName("ORDEN");
        builder.Property(x => x.Description).HasColumnName("DESCRIPCION");
        builder.Property(x => x.Notes).HasColumnName("OBSERVACIONES");
        builder.Property(x => x.ParentLedgerAccountId).HasColumnName("PADRE");
        builder.Property(x => x.AccountingPeriodId).HasColumnName("ID_EJERCICIO");
        builder.Property(x => x.NormalBalance)
               .HasColumnName("SALDO")
               .HasMaxLength(2)
               .HasConversion(CustomerConverters.NormalBalanceTypeToString);

        builder.Property(x => x.LedgerAccountType)
                   .HasColumnName("TIPO")
                   .HasConversion(CustomerConverters.LedgerAccountTypeToString);

        builder.HasOne(x => x.AccountingPeriod)
            .WithMany()
            .HasForeignKey(x => x.AccountingPeriodId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}