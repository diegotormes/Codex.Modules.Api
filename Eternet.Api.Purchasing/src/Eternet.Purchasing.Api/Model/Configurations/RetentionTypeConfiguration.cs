namespace Eternet.Purchasing.Api.Model.Configurations;

public static class RetentionTypeConfiguration
{
    public static void ConfigureRetentionType(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RetentionType>(entity =>
        {
            entity.ToTable("TIPOS_RETENCIONES");

            entity.HasKey(e => e.Key).HasName("PK_TIPOS_RETENCIONES");

            entity.Property(e => e.Key)
                  .HasColumnName("CLAVE");

            entity.Property(e => e.Description)
                  .HasColumnName("DESCRIPCION")
                  .HasColumnType($"varchar({RetentionTypeConstraints.DescriptionMaxLen})")
                  .HasMaxLength(RetentionTypeConstraints.DescriptionMaxLen);

            entity.Property(e => e.AccountId)
                  .HasColumnName("ID_CUENTA");

            entity.Property(e => e.MinimumTaxable)
                  .HasColumnName("MIN_IMPONIBLE")
                  .HasDefaultValue(0.0f);

            entity.Property(e => e.WithDetail)
                  .HasColumnName("CON_DETALLE")
                  .HasDefaultValue((short)0);
        });
    }
}