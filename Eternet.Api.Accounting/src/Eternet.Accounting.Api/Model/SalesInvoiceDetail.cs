using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public class SalesInvoiceDetail
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal VatRate { get; set; }
    public decimal Vat { get; set; }
}

public class SalesInvoiceDetailConfiguration : IEntityTypeConfiguration<SalesInvoiceDetail>
{
    public void Configure(EntityTypeBuilder<SalesInvoiceDetail> builder)
    {
        builder.ToTable("DETALLE_FC_VENTA");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");
        builder.Property(x => x.InvoiceId).HasColumnName("ID_FACTURA");
        builder.Property(x => x.Quantity).HasColumnName("CANTIDAD");
        builder.Property(x => x.Price).HasColumnName("PRECIO");
        builder.Property(x => x.VatRate).HasColumnName("PORC_IVA");
        builder.Property(x => x.Vat).HasColumnName("IVA");
    }
}
