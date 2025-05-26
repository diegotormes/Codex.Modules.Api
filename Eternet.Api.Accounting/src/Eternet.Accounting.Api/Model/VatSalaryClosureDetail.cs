using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Eternet.Models.Abstractions;

namespace Eternet.Accounting.Api.Model;

public class VatSalaryClosureDetail : IIntIdentity
{
    public int Id { get; set; }
    public required int SalaryClosureDetailId { get; set; }
    public required short Month { get; set; }
    public required short Year { get; set; }
    public decimal? Total { get; set; }
    public decimal? IvaCf { get; set; }
}

public class VatSalaryClosureDetailConfiguration : IEntityTypeConfiguration<VatSalaryClosureDetail>
{
    public void Configure(EntityTypeBuilder<VatSalaryClosureDetail> builder)
    {
        builder.ToTable("SUELDOS_CIERRE_DET_IVA");
        builder.Property(x => x.Id).HasColumnName("ID").ValueGeneratedOnAdd();
        builder.Property(x => x.SalaryClosureDetailId).HasColumnName("ID_CIERRE_DET");
        builder.Property(x => x.Month).HasColumnName("MES");
        builder.Property(x => x.Year).HasColumnName("ANIO");
        builder.Property(x => x.Total).HasColumnName("TOTAL");
        builder.Property(x => x.IvaCf).HasColumnName("IVA_CF");
    }
}
