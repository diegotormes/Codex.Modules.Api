namespace Eternet.Purchasing.Api.Model.Configurations;

public static class PurchaseRetentionConfiguration
{
    public static void ConfigurePurchaseRetention(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PurchaseRetention>(entity =>
        {
            entity.ToTable("RETENCIONES_COMPRAS");
            entity.HasKey(e => e.Id).HasName("PK_RETENCIONES_COMPRAS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.VatPurchaseId).HasColumnName("ID_IVA_COMPRA");
            entity.Property(e => e.RetentionCode)
                  .HasColumnName("CODIGO_RETENCION")
                  .HasColumnType("varchar(3)");
            entity.Property(e => e.CertificateNumber)
                  .HasColumnName("NRO_CERTIFICADO")
                  .HasColumnType("varchar(20)");
            entity.Property(e => e.Month).HasColumnName("MES");
            entity.Property(e => e.Year).HasColumnName("ANIO");
            entity.Property(e => e.RetentionType).HasColumnName("TIPO_RETENCION");
            entity.Property(e => e.PurchaseInvoiceRetentionId).HasColumnName("ID_FC_COMPRA_RET");

            entity.HasOne(d => d.VatPurchase)
                .WithMany(p => p.PurchaseRetentions)
                .HasForeignKey(d => d.VatPurchaseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RETENCIONES_COMPRAS_IVA");

            entity.HasOne(d => d.InvoicePurchaseRetention)
                .WithMany(p => p.Retentions)
                .HasForeignKey(d => d.PurchaseInvoiceRetentionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RETENCIONES_COMPRAS_RET");

            entity.HasOne(d => d.RetentionTypeReference)
                .WithMany()
                .HasForeignKey(d => d.RetentionType)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_RETENCIONES_COMPRAS_TIPORET");
        });
    }
}