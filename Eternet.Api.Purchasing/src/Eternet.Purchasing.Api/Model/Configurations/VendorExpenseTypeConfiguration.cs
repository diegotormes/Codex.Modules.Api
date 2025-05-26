namespace Eternet.Purchasing.Api.Model.Configurations;

public static class VendorExpenseTypeConfiguration
{
    public static void ConfigureVendorExpenseType(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VendorExpenseType>(entity =>
        {
            entity.ToTable("PROVS_TIPOS_GASTOS");
            entity.HasKey(e => e.Id).HasName("PK_PROVS_TIPOS_GASTOS");
            entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
            entity.Property(e => e.VendorId).HasColumnName("ID_PROVEEDOR");
            entity.Property(e => e.ExpenseTypeId).HasColumnName("ID_TIPO_GASTO");
        });
    }
}