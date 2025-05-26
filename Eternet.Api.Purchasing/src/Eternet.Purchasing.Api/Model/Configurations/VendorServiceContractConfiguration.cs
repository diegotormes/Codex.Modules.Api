namespace Eternet.Purchasing.Api.Model.Configurations;

public static class VendorServiceContractConfiguration
{
    public static void ConfigureVendorServiceContract(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VendorServiceContract>(entity =>
        {
            entity.ToTable("PROVS_SERVS_CONT");

            entity.HasKey(e => e.Id).HasName("PK_PROVS_SERVS_CONT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.VendorId).HasColumnName("ID_PROVEEDOR");
            entity.Property(e => e.ExpenseId).HasColumnName("ID_GASTO");
            entity.Property(e => e.Description)
                  .HasColumnName("DESCRIPCION")
                  .HasMaxLength(50);

            entity.Property(e => e.Months).HasColumnName("MESES");
            entity.Property(e => e.RegistrationDate).HasColumnName("FECHA_ALTA");

            entity.HasIndex(e => e.VendorId, "PROVS_SERVS_CONT_ID_PROV");

            entity.HasOne(d => d.ExpenseType)
                .WithMany(p => p.ProviderServiceContracts)
                .HasForeignKey(d => d.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PROVS_SERVS_CONT_GASTO");
        });
    }
}