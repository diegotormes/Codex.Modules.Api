namespace Eternet.Purchasing.Api.Model.Configurations;

public static class SalePointConfiguration
{
    public static void ConfigureSalePoint(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SalePoint>(entity =>
        {
            entity.ToTable("PUNTOS_VENTA");
            entity.Property(x => x.Id).HasColumnName("CLAVE");
            entity.Property(x => x.Description)
                  .HasColumnName("DESCRIPCION")
                  .HasColumnType($"varchar({SalePointConstraints.DescriptionMaxLen})")
                  .HasMaxLength(SalePointConstraints.DescriptionMaxLen);

            entity.Property(x => x.Service).HasColumnName("SERVICIO");
        });
    }
}