using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public class PurchaseInvoice
{
    public int Id { get; set; }
    public string VoucherType { get; set; } = string.Empty;
}

public class PurchaseInvoiceConfiguration : IEntityTypeConfiguration<PurchaseInvoice>
{
    public void Configure(EntityTypeBuilder<PurchaseInvoice> builder)
    {
        builder.ToTable("FACTURAS_COMPRA");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CLAVE");
        builder.Property(x => x.VoucherType).HasColumnName("TIPO_COMPROBANTE");
    }
}
