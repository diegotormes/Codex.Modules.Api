using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public class PurchaseRetentionDetail
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int RetentionTypeId { get; set; }
    public int? ProvinceId { get; set; }
    public decimal Amount { get; set; }
}

public class PurchaseRetentionDetailConfiguration : IEntityTypeConfiguration<PurchaseRetentionDetail>
{
    public void Configure(EntityTypeBuilder<PurchaseRetentionDetail> builder)
    {
        builder.ToTable("FC_COMPRA_RETS");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");
        builder.Property(x => x.InvoiceId).HasColumnName("ID_FACTURA");
        builder.Property(x => x.RetentionTypeId).HasColumnName("TIPO_RETENCION");
        builder.Property(x => x.ProvinceId).HasColumnName("ID_PROVINCIA");
        builder.Property(x => x.Amount).HasColumnName("IMPORTE");
    }
}
