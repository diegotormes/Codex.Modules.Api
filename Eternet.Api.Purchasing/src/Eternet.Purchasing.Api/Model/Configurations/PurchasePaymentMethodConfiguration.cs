namespace Eternet.Purchasing.Api.Model.Configurations;

public static class PurchasePaymentMethodConfiguration
{
    public static void ConfigurePurchasePaymentMethod(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PurchasePaymentMethod>(entity =>
        {
            entity.ToTable("FC_COMPRA_FORMA_PAGO");
            entity.HasKey(e => e.Id).HasName("PK_FC_COMPRA_FORMA_PAGO");

            entity.HasOne(p => p.PurchaseInvoice)
                .WithMany(b => b.PurchasePaymentMethods)
                .HasForeignKey(p => p.InvoiceId);

            entity.HasOne(p => p.PaymentMethod)
                .WithMany(b => b.PurchaseInvoicesPayments)
                .HasForeignKey(p => p.PaymentMethodId);

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.InvoiceId).HasColumnName("ID_FACTURA");
            entity.Property(e => e.Description)
                  .HasColumnName("DESCRIPCION")
                  .HasColumnType("varchar(20)");

            entity.Property(e => e.Date).HasColumnName("FECHA");
            entity.Property(e => e.TotalAmount)
                  .HasColumnName("IMPORTE")
                  .HasPrecision(10, 2);

            entity.Property(e => e.PaymentMethodId)
                  .HasColumnName("ID_FORMA_PAGO");
        });
    }
}