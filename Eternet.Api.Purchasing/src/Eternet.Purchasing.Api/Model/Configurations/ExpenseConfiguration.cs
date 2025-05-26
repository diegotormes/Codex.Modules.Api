namespace Eternet.Purchasing.Api.Model.Configurations;

public static class ExpenseConfiguration
{
    public static void ConfigureExpense(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.ToTable("GASTOS");
            entity.HasKey(e => e.Key).HasName("PK_GASTOS");
            entity.Property(e => e.ServiceType).HasColumnName("TIPO_SERVICIO");
            entity.Property(e => e.ExpenseType).HasColumnName("TIPO_GASTO");
            entity.Property(e => e.Date).HasColumnName("FECHA").HasDefaultValueSql("'TODAY'");
            entity.Property(e => e.Observations).HasColumnName("OBSERVACIONES").HasColumnType("varchar(60)");
            entity.Property(e => e.Provider).HasColumnName("PROVEEDOR");
            entity.Property(e => e.Amount).HasColumnName("MONTO");
            entity.Property(e => e.Paid).HasColumnName("PAGADO").HasColumnType("varchar(2)");
            entity.Property(e => e.DueDate).HasColumnName("FECHA_VENCIMIENTO").HasDefaultValueSql("'TODAY'");
            entity.Property(e => e.Receipt).HasColumnName("COMPROBANTE").HasColumnType("varchar(8)").HasDefaultValue("-1");
            entity.Property(e => e.PointOfSale).HasColumnName("PUNTO_VENTA").HasDefaultValue(-1);
            entity.Property(e => e.Branch).HasColumnName("SUCURSAL").HasColumnType("varchar(4)");
            entity.Property(e => e.VatDate).HasColumnName("FECHA_IVA");
            entity.Property(e => e.NonTaxable).HasColumnName("NO_GRAVADO");
            entity.Property(e => e.Exempt).HasColumnName("EXENTO");
            entity.Property(e => e.VatAmount).HasColumnName("IMPORTE_IVA");
            entity.Property(e => e.VatRetention).HasColumnName("RETENCION_IVA");
            entity.Property(e => e.IBRetention).HasColumnName("RETENCION_IB");
            entity.Property(e => e.EarningsRetention).HasColumnName("RETENCION_GAN");
            entity.Property(e => e.VatPerception).HasColumnName("PERCEPCION_IVA");
            entity.Property(e => e.IBPerception).HasColumnName("PERCEPCION_IB");
            entity.Property(e => e.Total).HasColumnName("TOTAL");
            entity.Property(e => e.VoucherType).HasColumnName("TIPO_COMPROBANTE").HasColumnType("varchar(2)");
            entity.Property(e => e.InvoiceType).HasColumnName("TIPO_FACTURA").HasColumnType("varchar(1)");
            entity.Property(e => e.ProviderName).HasColumnName("NOMPROV").HasColumnType("varchar(40)");
            entity.Property(e => e.InvoiceId).HasColumnName("ID_FACTURA");
            entity.Property(e => e.IsFictitious).HasColumnName("FICTICIO").HasDefaultValue((short)0);
            entity.Property(e => e.PurchaseInvoiceDetailId).HasColumnName("ID_DETALLE_FC_COMPRA");
        });
    }
}