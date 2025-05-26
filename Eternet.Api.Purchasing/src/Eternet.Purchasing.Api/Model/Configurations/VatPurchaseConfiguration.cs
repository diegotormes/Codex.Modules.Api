namespace Eternet.Purchasing.Api.Model.Configurations;

public static class VatPurchaseConfiguration
{
    public static void ConfigureVatPurchase(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VatPurchase>(entity =>
        {
            entity.ToTable("IVA_COMPRA");
            entity.HasKey(e => e.Id).HasName("PK_IVA_COMPRA");

            entity.HasOne(p => p.PurchaseInvoice)
                .WithMany(b => b.VatPurchases)
                .HasForeignKey(p => p.InvoiceId);

            entity.Property(e => e.Id)
                  .HasColumnName("CLAVE");

            entity.Property(e => e.InvoiceId)
                  .HasColumnName("ID_FACTURA");

            entity.Property(e => e.SalesPoint)
                  .HasColumnName("PUNTO_VENTA");

            entity.Property(e => e.VoucherType)
                  .HasColumnName("TIPO_COMPROBANTE")
                  .HasColumnType($"varchar({VatPurchaseConstraints.VoucherTypeMaxLen})")
                  .HasMaxLength(VatPurchaseConstraints.VoucherTypeMaxLen);

            entity.Property(e => e.InvoiceType)
                  .HasColumnName("TIPO_FACTURA")
                  .HasColumnType($"varchar({VatPurchaseConstraints.InvoiceTypeMaxLen})")
                  .HasMaxLength(VatPurchaseConstraints.InvoiceTypeMaxLen);

            entity.Property(e => e.BranchNumber)
                  .HasColumnName("NUMERO_SUCURSAL")
                  .HasColumnType($"varchar({VatPurchaseConstraints.BranchNumberMaxLen})")
                  .HasMaxLength(VatPurchaseConstraints.BranchNumberMaxLen);

            entity.Property(e => e.VoucherNumber)
                  .HasColumnName("NUMERO_COMPROBANTE")
                  .HasColumnType($"varchar({VatPurchaseConstraints.VoucherNumberMaxLen})")
                  .HasMaxLength(VatPurchaseConstraints.VoucherNumberMaxLen);

            entity.Property(e => e.VatTaxDate)
                  .HasColumnName("FECHA_IVA");

            entity.Property(e => e.InvoiceDate)
                  .HasColumnName("FECHA_FACTURA");

            entity.Property(e => e.VendorName)
                  .HasColumnName("PROVEEDOR")
                  .HasColumnType($"varchar({VatPurchaseConstraints.VendorNameMaxLen})")
                  .HasMaxLength(VatPurchaseConstraints.VendorNameMaxLen);

            entity.Property(e => e.Cuit)
                  .HasColumnName("CUIT")
                  .HasColumnType($"varchar({VatPurchaseConstraints.CuitMaxLen})")
                  .HasMaxLength(VatPurchaseConstraints.CuitMaxLen);

            entity.Property(e => e.VatSituation)
                  .HasColumnName("RESPONS_IVA")
                  .HasColumnType($"varchar({VatPurchaseConstraints.VatSituationMaxLen})")
                  .HasMaxLength(VatPurchaseConstraints.VatSituationMaxLen);

            entity.Property(e => e.VatRate)
                  .HasColumnName("TASA_IVA")
                  .HasPrecision(10, 2);

            entity.Property(e => e.Taxable)
                  .HasColumnName("GRAVADO")
                  .HasPrecision(10, 2);

            entity.Property(e => e.NonTaxable)
                  .HasColumnName("NO_GRAVADO")
                  .HasPrecision(10, 2);

            entity.Property(e => e.Exempt)
                  .HasColumnName("EXENTO")
                  .HasPrecision(10, 2);

            entity.Property(e => e.VatTotalAmount)
                  .HasColumnName("IVA")
                  .HasPrecision(10, 2);

            entity.Property(e => e.VatRetentionTotalAmount)
                  .HasColumnName("RETENCION_IVA")
                  .HasPrecision(10, 2);

            entity.Property(e => e.IbRetentionTotalAmount)
                  .HasColumnName("RETENCION_IB")
                  .HasPrecision(10, 2);

            entity.Property(e => e.ProfitRetentionTotalAmount)
                  .HasColumnName("RETENCION_GAN")
                  .HasPrecision(10, 2);

            entity.Property(e => e.VatPerceptionTotalAmount)
                  .HasColumnName("PERCEPCION_IVA")
                  .HasPrecision(10, 2);

            entity.Property(e => e.IbPerceptionTotalAmount)
                  .HasColumnName("PERCEPCION_IB")
                  .HasPrecision(10, 2);

            entity.Property(e => e.PurchasesC)
                  .HasColumnName("COMPRAS_C")
                  .HasPrecision(10, 2);

            entity.Property(e => e.TotalAmount)
                  .HasColumnName("TOTAL")
                  .HasPrecision(10, 2);

            entity.Property(e => e.CreditCardLiquidation)
                  .HasColumnName("LIQUIDACIONTJ");

            entity.Property(e => e.ProfitPerception)
                  .HasColumnName("PERCEPCION_GAN")
                  .HasDefaultValue(0)
                  .HasPrecision(10, 2);

            entity.Property(e => e.VatRetentionPerception)
                  .HasColumnName("RET_PERC_IVA")
                  .HasPrecision(10, 2);
        });
    }
}