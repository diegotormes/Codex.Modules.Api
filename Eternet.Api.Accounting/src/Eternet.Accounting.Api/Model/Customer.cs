using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
}

public class ClientConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("CLIENTES");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CLAVE");
        builder.Property(x => x.Name).HasColumnName("AYN");
        builder.Property(x => x.TaxNumber).HasColumnName("CUIT");
    }
}
