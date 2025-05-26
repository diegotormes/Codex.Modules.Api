using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public class SalesInvoice
{
    public int Id { get; set; }
    public short VoucherType { get; set; }
    public string InvoiceType { get; set; } = string.Empty;
    public short PointOfSale { get; set; }
    public int Number { get; set; }
    public DateOnly Date { get; set; }
    public string TaxResponsibility { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
}

public class SalesInvoiceConfiguration : IEntityTypeConfiguration<SalesInvoice>
{
    public void Configure(EntityTypeBuilder<SalesInvoice> builder)
    {
        builder.ToTable("FACTURAS_VTA");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CLAVE");
        builder.Property(x => x.VoucherType).HasColumnName("TIPO_COMPROBANTE");
        builder.Property(x => x.InvoiceType).HasColumnName("TIPO_FACTURA");
        builder.Property(x => x.PointOfSale).HasColumnName("PUNTO_VENTA");
        builder.Property(x => x.Number).HasColumnName("NUMERO_FACTURA");
        builder.Property(x => x.Date).HasColumnName("FECHA");
        builder.Property(x => x.TaxResponsibility).HasColumnName("IVA");
        builder.Property(x => x.TaxNumber).HasColumnName("CUIT");
    }
}
