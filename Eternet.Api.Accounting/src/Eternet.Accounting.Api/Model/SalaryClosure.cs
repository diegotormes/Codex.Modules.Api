using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eternet.Accounting.Api.Model;

public class SalaryClosure
{
    public int Id { get; set; }
    public required DateOnly CloseDate { get; set; }
    public required short Month { get; set; }
    public required short Year { get; set; }
    public decimal? BaseCalculationCfIva { get; set; }

    public ICollection<SalaryClosureDetail> Details { get; set; } = [];
}

public class SalaryClosureConfiguration : IEntityTypeConfiguration<SalaryClosure>
{
    public void Configure(EntityTypeBuilder<SalaryClosure> builder)
    {
        builder.ToTable("SUELDOS_CIERRE");
        builder.Property(x => x.Id).HasColumnName("ID").ValueGeneratedOnAdd();
        builder.Property(x => x.CloseDate).HasColumnName("FECHA_CIERRE");
        builder.Property(x => x.Month).HasColumnName("MES");
        builder.Property(x => x.Year).HasColumnName("ANIO");
        builder.Property(x => x.BaseCalculationCfIva).HasColumnName("BASE_CALC_CF_IVA");

        builder.HasMany(x => x.Details)
            .WithOne(x => x.Closure)
            .HasForeignKey(x => x.ClosureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
