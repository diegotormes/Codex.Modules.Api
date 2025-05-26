using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public class VatPurchase
{
    public int Id { get; set; }
    public DateOnly InvoiceDate { get; set; }
    public DateOnly VatDate { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string Cuit { get; set; } = string.Empty;
    public decimal VatRate { get; set; }
    public decimal TaxableAmount { get; set; }
    public decimal VatAmount { get; set; }
    public decimal Total { get; set; }
    public decimal? VatRetention { get; set; }
    public decimal? VatPerception { get; set; }
    public int? InvoiceId { get; set; }
    public PurchaseInvoice? Invoice { get; set; }
}

public class VatPurchaseConfiguration : IEntityTypeConfiguration<VatPurchase>
{
    public void Configure(EntityTypeBuilder<VatPurchase> builder)
    {
        builder.ToTable("IVA_COMPRA");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CLAVE");
        builder.Property(x => x.InvoiceDate).HasColumnName("FECHA_FACTURA");
        builder.Property(x => x.VatDate).HasColumnName("FECHA_IVA");
        builder.Property(x => x.Provider).HasColumnName("PROVEEDOR");
        builder.Property(x => x.Cuit).HasColumnName("CUIT");
        builder.Property(x => x.VatRate).HasColumnName("TASA_IVA");
        builder.Property(x => x.TaxableAmount).HasColumnName("GRAVADO");
        builder.Property(x => x.VatAmount).HasColumnName("IVA");
        builder.Property(x => x.Total).HasColumnName("TOTAL");
        builder.Property(x => x.VatRetention).HasColumnName("RETENCION_IVA");
        builder.Property(x => x.VatPerception).HasColumnName("PERCEPCION_IVA");
        builder.Property(x => x.InvoiceId).HasColumnName("ID_FACTURA");

        builder.HasOne(x => x.Invoice)
            .WithMany()
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
