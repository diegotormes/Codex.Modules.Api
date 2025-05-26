using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public class SalaryItem
{
    public short Id { get; set; }
    public required string Description { get; set; } = string.Empty;
    public short Type { get; set; }
    public short IsPercentage { get; set; }
    public decimal Value { get; set; }
    public short? ConsidersNorem { get; set; }
    public short? OnlyAffiliates { get; set; }
    public short? ContributionType { get; set; }
    public int SocialSecurity { get; set; }
    public decimal? Value2 { get; set; }
    public short ExemptManager { get; set; }
    public string? AfipItem { get; set; }
    public string? UnitItem { get; set; }
    public short? Multiple { get; set; }
    public short? IsBonus { get; set; }
}

public class SalaryItemConfiguration : IEntityTypeConfiguration<SalaryItem>
{
    public void Configure(EntityTypeBuilder<SalaryItem> builder)
    {
        builder.ToTable("SUELDOS_ITEMS");
        builder.Property(x => x.Id).HasColumnName("ID");
        builder.Property(x => x.Description).HasColumnName("DESCRIPCION");
        builder.Property(x => x.Type).HasColumnName("TIPO");
        builder.Property(x => x.IsPercentage).HasColumnName("ES_PORC");
        builder.Property(x => x.Value).HasColumnName("VALOR");
        builder.Property(x => x.ConsidersNorem).HasColumnName("CONSIDERA_NOREM");
        builder.Property(x => x.OnlyAffiliates).HasColumnName("SOLO_AFILIADOS");
        builder.Property(x => x.ContributionType).HasColumnName("TIPO_APORTE");
        builder.Property(x => x.SocialSecurity).HasColumnName("OBRA_SOCIAL");
        builder.Property(x => x.Value2).HasColumnName("VALOR2");
        builder.Property(x => x.ExemptManager).HasColumnName("EXENTO_GERENTE");
        builder.Property(x => x.AfipItem).HasColumnName("ITEM_AFIP");
        builder.Property(x => x.UnitItem).HasColumnName("ITEM_UNIDAD");
        builder.Property(x => x.Multiple).HasColumnName("MULTIPLE");
        builder.Property(x => x.IsBonus).HasColumnName("ES_BONO");
    }
}
