using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public class RetentionSale
{
    public int Id { get; set; }
    public int MovementId { get; set; }
    public int RetentionTypeId { get; set; }
    public DateOnly Date { get; set; }
    public decimal Amount { get; set; }
    public string RetentionCode { get; set; } = string.Empty;
    public string? CertificateNumber { get; set; }
    public short Month { get; set; }
    public short Year { get; set; }

    public RetentionType RetentionType { get; set; } = null!;
    public CustomerAccount Movement { get; set; } = null!;
}

public class RetentionSaleConfiguration : IEntityTypeConfiguration<RetentionSale>
{
    public void Configure(EntityTypeBuilder<RetentionSale> builder)
    {
        builder.ToTable("RETENCIONES_VENTAS");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CLAVE");
        builder.Property(x => x.MovementId).HasColumnName("NRO_MOVIMIENTO");
        builder.Property(x => x.RetentionTypeId).HasColumnName("TIPO_RETENCION");
        builder.Property(x => x.Date).HasColumnName("FECHA");
        builder.Property(x => x.Amount).HasColumnName("IMPORTE");
        builder.Property(x => x.RetentionCode).HasColumnName("CODIGO_RETENCION");
        builder.Property(x => x.CertificateNumber).HasColumnName("NRO_CERTIFICADO");
        builder.Property(x => x.Month).HasColumnName("MES");
        builder.Property(x => x.Year).HasColumnName("ANIO");

        builder.HasOne(x => x.RetentionType)
            .WithMany()
            .HasForeignKey(x => x.RetentionTypeId);

        builder.HasOne(x => x.Movement)
            .WithMany()
            .HasForeignKey(x => x.MovementId);
    }
}
