using Eternet.Purchasing.Api.Converters;

namespace Eternet.Purchasing.Api.Model.Configurations;

public static class ExpenseTypeConfiguration
{
    public static void ConfigureExpenseType(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExpenseType>(entity =>
        {
            entity.HasQueryFilter(et => et.IsActive);
            entity.ToTable("TIPOS_GASTOS");
            entity.HasKey(e => e.Id).HasName("PK_TIPOS_GASTOS");
            entity.Property(e => e.Id).HasColumnName("CLAVE");

            entity.Property(e => e.Description).HasColumnName("DESCRIPCION").HasColumnType("varchar(30)");
            entity.Property(e => e.IsActive)
                .HasColumnName("ACTIVO")
                .HasConversion(
                    v => EFShortToBoolConverterExtensions.ToLocalDbModel(v),
                    v => EFShortToBoolConverterExtensions.ToLocalDomainModel(v));
            entity.Property(e => e.AccountId).HasColumnName("ID_CUENTA");
            entity.Property(e => e.VatRate).HasColumnName("TASA_IVA").HasDefaultValue(21.0f);
            entity.Property(e => e.EarningTaxId).HasColumnName("TIPO_RETENCION_GAN");
            entity.Property(e => e.IncludeInPurchases).HasColumnName("INCLUIR_COMPRAS").HasDefaultValue((short)1);
            entity.Property(e => e.KeyString).HasColumnName("CLAVE_STR").HasColumnType("varchar(15)");
        });
    }
}