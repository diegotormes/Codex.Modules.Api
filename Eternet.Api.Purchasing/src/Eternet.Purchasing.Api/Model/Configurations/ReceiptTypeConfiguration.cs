namespace Eternet.Purchasing.Api.Model.Configurations;

public static class ReceiptTypeConfiguration
{
    public static void ConfigureReceiptType(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReceiptType>(entity =>
        {
            entity.ToTable("TIPOS_COMPROBANTE");
            entity.Property(x => x.Id).HasColumnName("CLAVE");
            entity.Property(x => x.Description)
                  .HasColumnName("DESCRIPCION")
                  .HasColumnType($"varchar({ReceiptTypeConstraints.DescriptionMaxLen})")
                  .HasMaxLength(ReceiptTypeConstraints.DescriptionMaxLen);
        });
    }
}