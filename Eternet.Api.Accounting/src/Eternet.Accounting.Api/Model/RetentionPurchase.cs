using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public class RetentionPurchase
{
    public int Id { get; set; }
    public int VatPurchaseId { get; set; }
    public string RetentionCode { get; set; } = string.Empty;
    public string? CertificateNumber { get; set; }
    public short Month { get; set; }
    public short Year { get; set; }
    public int RetentionTypeId { get; set; }
    public int? PurchaseRetentionDetailId { get; set; }

    public VatPurchase VatPurchase { get; set; } = null!;
    public RetentionType RetentionType { get; set; } = null!;
    public PurchaseRetentionDetail? PurchaseRetentionDetail { get; set; }
}

public class RetentionPurchaseConfiguration : IEntityTypeConfiguration<RetentionPurchase>
{
    public void Configure(EntityTypeBuilder<RetentionPurchase> builder)
    {
        builder.ToTable("RETENCIONES_COMPRAS");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ID");
        builder.Property(x => x.VatPurchaseId).HasColumnName("ID_IVA_COMPRA");
        builder.Property(x => x.RetentionCode).HasColumnName("CODIGO_RETENCION");
        builder.Property(x => x.CertificateNumber).HasColumnName("NRO_CERTIFICADO");
        builder.Property(x => x.Month).HasColumnName("MES");
        builder.Property(x => x.Year).HasColumnName("ANIO");
        builder.Property(x => x.RetentionTypeId).HasColumnName("TIPO_RETENCION");
        builder.Property(x => x.PurchaseRetentionDetailId).HasColumnName("ID_FC_COMPRA_RET");

        builder.HasOne(x => x.VatPurchase)
            .WithMany()
            .HasForeignKey(x => x.VatPurchaseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.RetentionType)
            .WithMany()
            .HasForeignKey(x => x.RetentionTypeId);

        builder.HasOne(x => x.PurchaseRetentionDetail)
            .WithMany()
            .HasForeignKey(x => x.PurchaseRetentionDetailId);
    }
}
