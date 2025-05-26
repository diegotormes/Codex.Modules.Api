using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public class RetentionType
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class RetentionTypeConfiguration : IEntityTypeConfiguration<RetentionType>
{
    public void Configure(EntityTypeBuilder<RetentionType> builder)
    {
        builder.ToTable("TIPOS_RETENCIONES");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CLAVE");
        builder.Property(x => x.Description).HasColumnName("DESCRIPCION");
    }
}
