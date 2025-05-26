namespace Eternet.Purchasing.Api.Model.Configurations;

public static class PurchaseInvoiceConfiguration
{
    public static void ConfigurePurchaseInvoice(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("FACTURAS_COMPRA");

            entity.HasKey(e => e.Id).HasName("PK_FACTURAS_COMPRA");

            entity.Property(e => e.Id)
                .HasColumnName("CLAVE")
                .ValueGeneratedOnAdd();

            entity.Property(x => x.Guid).HasColumnName("GUID").HasColumnType("CHAR(16) CHARACTER SET OCTETS");

            entity.Property(e => e.InvoiceType)
                  .HasColumnName("TIPO_FACTURA")
                  .HasColumnType($"varchar({PurchaseInvoiceConstraints.InvoiceTypeMaxLen})")
                  .HasMaxLength(PurchaseInvoiceConstraints.InvoiceTypeMaxLen);

            entity.Property(e => e.VoucherType)
                  .HasColumnName("TIPO_COMPROBANTE")
                  .HasColumnType($"varchar({PurchaseInvoiceConstraints.VoucherTypeMaxLen})")
                  .HasMaxLength(PurchaseInvoiceConstraints.VoucherTypeMaxLen);

            entity.Property(e => e.BranchNumber)
                  .HasColumnName("NUMERO_SUCURSAL")
                  .HasColumnType($"varchar({PurchaseInvoiceConstraints.BranchNumberMaxLen})")
                  .HasMaxLength(PurchaseInvoiceConstraints.BranchNumberMaxLen);

            entity.Property(e => e.VoucherNumber)
                  .HasColumnName("NUMERO_COMPROBANTE")
                  .HasColumnType($"varchar({PurchaseInvoiceConstraints.VoucherNumberMaxLen})")
                  .HasMaxLength(PurchaseInvoiceConstraints.VoucherNumberMaxLen);

            entity.Property(e => e.PaymentMethodId)
                  .HasColumnName("FORMA_PAGO");

            entity.Property(e => e.IssueDate)
                  .HasColumnName("FECHA_FACTURA");

            entity.Property(e => e.DueDate)
                  .HasColumnName("FECHA_VENCIMIENTO");

            entity.Property(e => e.SalesPoint)
                  .HasColumnName("PUNTO_VENTA");

            entity.Property(e => e.Description)
                  .HasColumnName("DESCRIPCION")
                  .HasColumnType($"varchar({PurchaseInvoiceConstraints.DescriptionMaxLen})")
                  .HasMaxLength(PurchaseInvoiceConstraints.DescriptionMaxLen);

            entity.Property(e => e.VendorId)
                  .HasColumnName("NUMERO_PROVEEDOR");

            entity.Property(e => e.VendorName)
                  .HasColumnName("NOMBRE_PROVEEDOR")
                  .HasColumnType($"varchar({PurchaseInvoiceConstraints.VendorNameMaxLen})")
                  .HasMaxLength(PurchaseInvoiceConstraints.VendorNameMaxLen);

            entity.Property(e => e.VendorPostalAddress)
                  .HasColumnName("DIRECCION")
                  .HasColumnType($"varchar({PurchaseInvoiceConstraints.VendorPostalAddressMaxLen})")
                  .HasMaxLength(PurchaseInvoiceConstraints.VendorPostalAddressMaxLen);

            entity.Property(e => e.VendorTaxId)
                  .HasColumnName("CUIT")
                  .HasColumnType($"varchar({PurchaseInvoiceConstraints.CuitMaxLen})")
                  .HasMaxLength(PurchaseInvoiceConstraints.CuitMaxLen);

            entity.Property(e => e.VatStatus)
                  .HasColumnName("RESPONS_IVA");

            entity.Property(e => e.Subtotal)
                  .HasColumnName("SUBTOTAL")
                  .HasPrecision(10, 2);

            entity.Property(e => e.VatTotalAmount)
                  .HasColumnName("IVA_RI")
                  .HasPrecision(10, 2);

            entity.Property(e => e.TotalAmount)
                  .HasColumnName("TOTAL")
                  .HasPrecision(10, 2);

            entity.Property(e => e.NetAmount)
                  .HasColumnName("NETO")
                  .HasPrecision(10, 2);

            entity.Property(e => e.ExemptAmount)
                  .HasColumnName("EXENTO")
                  .HasPrecision(10, 2);

            entity.Property(e => e.NonTaxableAmount)
                  .HasColumnName("NO_GRAVADO")
                  .HasPrecision(10, 2);

            entity.Property(e => e.Discount)
                  .HasColumnName("DESCUENTO");

            entity.Property(e => e.Surcharge)
                  .HasColumnName("RECARGO");

            entity.Property(e => e.TotalPayment)
                  .HasColumnName("TOTAL_PAGO");

            entity.Property(e => e.VatTaxDate)
                  .HasColumnName("FECHA_IVA");

            entity.Property(e => e.AmountToPay)
                  .HasColumnName("IMPORTE_A_PAGAR");

            entity.Property(e => e.PayAll)
                  .HasColumnName("PAGA_TODO");

            entity.Property(e => e.ExchangeRate)
                  .HasColumnName("DOLAR")
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

            entity.Property(e => e.ProfitPerceptionTotalAmount)
                  .HasColumnName("PERCEPCION_GAN")
                  .HasPrecision(10, 2);

            entity.Property(e => e.TotalReference)
                  .HasColumnName("TOTAL_REF");

            entity.Property(e => e.Barcode)
                  .HasColumnName("CODIGO_BARRAS")
                  .HasColumnType($"varchar({PurchaseInvoiceConstraints.BarcodeMaxLen})")
                  .HasMaxLength(PurchaseInvoiceConstraints.BarcodeMaxLen);

            entity.Property(e => e.ServiceContractId)
                  .HasColumnName("ID_SERV_CONT");

            entity.Property(e => e.Month)
                  .HasColumnName("MES");

            entity.Property(e => e.Year)
                  .HasColumnName("ANIO");

            entity.Property(e => e.SharePointFile)
                  .HasColumnName("SHAREPOINT_FILE")
                  .HasColumnType($"varchar({PurchaseInvoiceConstraints.SharePointFileMaxLen})")
                  .HasMaxLength(PurchaseInvoiceConstraints.SharePointFileMaxLen);

            entity.HasOne(p => p.VendorServiceContract)
                  .WithMany(b => b.PurchaseInvoices)
                  .HasForeignKey(p => p.ServiceContractId)
                  .IsRequired(false);

        });
    }
}