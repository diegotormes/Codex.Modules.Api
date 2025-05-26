namespace Eternet.Purchasing.Api.Model.Configurations;

public static class PurchaseInvoiceDetailConfiguration
{
    public static void ConfigurePurchaseInvoiceDetail(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.ToTable("DETALLE_FC_COMPRA");
            entity.HasKey(e => e.Id).HasName("PK_DETALLE_FC_COMPRA");

            entity.HasOne(p => p.PurchaseInvoice)
                .WithMany(b => b.Details)
                .HasForeignKey(p => p.InvoiceId);

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.InvoiceId).HasColumnName("CLAVE_FACTURA");

            entity.Property(e => e.ServiceArticleId)
                  .HasColumnName("CODIGO_ART_SERV")
                  .HasColumnType($"varchar({PurchaseInvoiceDetailConstraints.ServiceArticleIdMaxLen})")
                  .HasMaxLength(PurchaseInvoiceDetailConstraints.ServiceArticleIdMaxLen);

            entity.Property(e => e.Description)
                  .HasColumnName("DESCRIPCION")
                  .HasColumnType($"varchar({PurchaseInvoiceDetailConstraints.DescriptionMaxLen})")
                  .HasMaxLength(PurchaseInvoiceDetailConstraints.DescriptionMaxLen);

            entity.Property(e => e.Quantity)
                  .HasColumnName("CANTIDAD");

            entity.Property(e => e.VatRate)
                  .HasColumnName("TASA_IVA")
                  .HasPrecision(10, 2);

            entity.Property(e => e.Taxable)
                  .HasColumnName("GRAVADO")
                  .HasPrecision(10, 2);

            entity.Property(e => e.Subtotal)
                  .HasColumnName("SUBTOTAL")
                  .HasPrecision(10, 2);

            entity.Property(e => e.Vat)
                  .HasColumnName("IVA")
                  .HasPrecision(10, 2);

            entity.Property(e => e.NonTaxable)
                  .HasColumnName("NO_GRAVADO")
                  .HasPrecision(10, 2);

            entity.Property(e => e.Exempt)
                  .HasColumnName("EXENTO")
                  .HasPrecision(10, 2);

            entity.Property(e => e.TotalDetail)
                  .HasColumnName("TOTAL_REF")
                  .HasPrecision(10, 2);

            entity.Property(e => e.AmountToPay)
                  .HasColumnName("IMPORTE_A_PAGAR")
                  .HasDefaultValue(0f);

            entity.Property(e => e.PaysAll)
                  .HasColumnName("PAGA_TODO")
                  .HasDefaultValue(1);

            entity.Property(e => e.Paid)
                  .HasColumnName("PAGADO")
                  .HasDefaultValue(0f);

            entity.Property(e => e.ExpenseArticle)
                  .HasColumnName("ARTICULO_GASTO");

            entity.Property(e => e.ArticleId)
                  .HasColumnName("ID_ARTICULO");

            entity.Property(e => e.ServiceId)
                  .HasColumnName("ID_SERVICIO");

            entity.Property(e => e.OrderDetailId)
                  .HasColumnName("ID_DETALLE_PEDIDO");

            entity.Property(e => e.Retentions)
                  .HasColumnName("RETENCIONES")
                  .HasDefaultValue(0)
                  .HasPrecision(10, 2);

            entity.Property(e => e.AccountableNet)
                  .HasColumnName("NETO_CONTABLE");
        });
    }
}