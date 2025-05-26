namespace Eternet.Purchasing.Api.Model.Configurations;

public static class CityConfiguration
{
    public static void ConfigureCity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("LOCALIDADES");
            entity.Property(x => x.Id).HasColumnName("ID");
            entity.Property(x => x.Name).HasColumnName("DESCRIPCION");
        });
    }
}