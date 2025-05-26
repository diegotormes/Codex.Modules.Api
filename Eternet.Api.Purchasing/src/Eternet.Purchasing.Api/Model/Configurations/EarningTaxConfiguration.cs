namespace Eternet.Purchasing.Api.Model.Configurations;

public static class EarningTaxConfiguration
{
    public static void ConfigureEarningTax(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EarningTax>(entity =>
        {
            entity.ToTable("AFIP_GANANCIAS");
            entity.Property(x => x.Id).HasColumnName("ID");
            entity.Property(x => x.Description).HasColumnName("DESCRIPCION");
            entity.Property(x => x.RetentionPercentage).HasColumnName("PORC_RETENCION");
            entity.Property(x => x.TaxableMinimum).HasColumnName("MINIMO_IMPONIBLE");
            entity.Property(x => x.MinimumRetention).HasColumnName("RETENCION_MININA");
        });
    }
}