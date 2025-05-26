namespace Eternet.Purchasing.Api.Model.Configurations;

public static class InvoicePurchaseRetentionConfiguration
{
    public static void ConfigureInvoicePurchaseRetention(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InvoicePurchaseRetention>(entity =>
        {
            entity.ToTable("FC_COMPRA_RETS");
            entity.HasKey(e => e.Id).HasName("PK_FC_COMPRA_RETS");

            entity.HasOne(p => p.PurchaseInvoice)
                .WithMany(b => b.PurchaseRetentions)
                .HasForeignKey(p => p.InvoiceId);

            entity.HasOne(p => p.Province)
                .WithMany(b => b.InvoicePurchaseRetention)
                .HasForeignKey(p => p.ProvinceId)
                .IsRequired(false);

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.InvoiceId).HasColumnName("ID_FACTURA");
            entity.Property(e => e.RetentionType).HasColumnName("TIPO_RETENCION");
            entity.Property(e => e.ProvinceId).HasColumnName("ID_PROVINCIA");
            entity.Property(e => e.TotalAmount)
                  .HasColumnName("IMPORTE")
                  .HasPrecision(10, 2);
        });
    }
}