namespace Eternet.Purchasing.Api.Model.Configurations;

public static class VendorCurrentAccountConfiguration
{
    public static void ConfigureVendorCurrentAccount(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VendorCurrentAccount>(entity =>
        {
            entity.ToTable("CTASCTES_PROVS");
            entity.HasKey(e => e.Id).HasName("PK_CTASCTES_PROVS");

            entity.HasOne(p => p.PurchaseInvoice)
                .WithMany(b => b.VendorCurrentAccounts)
                .HasForeignKey(p => p.InvoiceId);

            entity.Property(e => e.Id).HasColumnName("CLAVE");
            entity.Property(e => e.Provider).HasColumnName("PROVEEDOR");
            entity.Property(e => e.Date)
                  .HasColumnName("FECHA")
                  .HasDefaultValueSql("'TODAY'");
            entity.Property(e => e.InvoiceType)
                  .HasColumnName("TIPO_FACTURA")
                  .HasColumnType("varchar(1)");
            entity.Property(e => e.VoucherType)
                  .HasColumnName("TIPO_COMPROBANTE")
                  .HasColumnType("varchar(2)");
            entity.Property(e => e.BranchNumber)
                  .HasColumnName("NUMERO_SUCURSAL")
                  .HasColumnType("varchar(4)");
            entity.Property(e => e.VoucherNumber)
                  .HasColumnName("NUMERO_COMPROBANTE")
                  .HasColumnType("varchar(8)");
            entity.Property(e => e.Debit)
                  .HasColumnName("DEBE");
            entity.Property(e => e.Credit)
                  .HasColumnName("HABER")
                  .HasPrecision(10, 2);
            entity.Property(e => e.Balance)
                  .HasColumnName("SALDO");
            entity.Property(e => e.Observations)
                  .HasColumnName("OBSERVACIONES")
                  .HasColumnType("varchar(100)");
            entity.Property(e => e.InvoiceId)
                  .HasColumnName("ID_FACTURA");
            entity.Property(e => e.PointOfSale)
                  .HasColumnName("PUNTO_VENTA");
            entity.Property(e => e.AdvancePayment)
                  .HasColumnName("ANTICIPO")
                  .HasDefaultValue(0f);
            entity.Property(e => e.User)
                  .HasColumnName("USUARIO")
                  .HasColumnType("varchar(20)");
            entity.Property(e => e.Security)
                  .HasColumnName("SEGURIDAD")
                  .HasDefaultValue((short)0);
        });
    }
}