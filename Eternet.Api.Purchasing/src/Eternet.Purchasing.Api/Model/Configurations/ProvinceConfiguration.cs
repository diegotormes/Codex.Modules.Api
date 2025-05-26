namespace Eternet.Purchasing.Api.Model.Configurations;

public static class ProvinceConfiguration
{
    public static void ConfigureProvince(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Province>(entity =>
        {
            entity.ToTable("PROVINCIAS");
            entity.HasKey(p => p.Id);

            entity.Property(e => e.Id)
                  .HasColumnName("ID");

            entity.Property(e => e.Description)
                  .HasColumnName("DESCRIPCION")
                  .HasMaxLength(ProvinceConstraints.DescriptionMaxLen)
                  .HasColumnType($"varchar({ProvinceConstraints.DescriptionMaxLen})");
        });
    }
}