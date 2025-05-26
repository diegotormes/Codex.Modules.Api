using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public class SalaryClosureDetail
{
    public int Id { get; set; }
    public required int ClosureId { get; set; }
    public SalaryClosure Closure { get; set; } = null!;
    public required int ProviderId { get; set; }
    public required string Description { get; set; } = string.Empty;
    public required int AccountId { get; set; }
    public decimal Amount { get; set; }
    public DateOnly DueDate { get; set; }
    public int? InvoiceId { get; set; }

    public ICollection<VatSalaryClosureDetail> VatDetails { get; set; } = [];
}

public class SalaryClosureDetailConfiguration : IEntityTypeConfiguration<SalaryClosureDetail>
{
    public void Configure(EntityTypeBuilder<SalaryClosureDetail> builder)
    {
        builder.ToTable("SUELDOS_CIERRE_DET");
        builder.Property(x => x.Id).HasColumnName("ID").ValueGeneratedOnAdd();
        builder.Property(x => x.ClosureId).HasColumnName("ID_CIERRE");
        builder.Property(x => x.ProviderId).HasColumnName("ID_PROVEEDOR");
        builder.Property(x => x.Description).HasColumnName("DESCRIPCION");
        builder.Property(x => x.AccountId).HasColumnName("ID_CUENTA");
        builder.Property(x => x.Amount).HasColumnName("MONTO");
        builder.Property(x => x.DueDate).HasColumnName("FECHA_VTO");
        builder.Property(x => x.InvoiceId).HasColumnName("ID_FACTURA");

        builder.HasOne(x => x.Closure)
            .WithMany(x => x.Details)
            .HasForeignKey(x => x.ClosureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
