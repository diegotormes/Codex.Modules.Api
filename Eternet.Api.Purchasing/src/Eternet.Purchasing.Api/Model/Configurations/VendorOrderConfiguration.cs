namespace Eternet.Purchasing.Api.Model.Configurations;

public static class VendorOrderConfiguration
{
    public static void ConfigureVendorOrder(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VendorOrder>(entity =>
        {
            entity.ToTable("PEDIDOS_PROV");
            entity.HasKey(e => e.Key).HasName("PK_PEDIDOS_PROV");

            entity.Property(e => e.OrderDate).HasColumnName("FECHA_PEDIDO");
            entity.Property(e => e.Buyer).HasColumnName("COMPRADOR");

            entity.Property(e => e.Transport)
                  .HasColumnName("TRANSPORTE")
                  .HasColumnType("varchar(30)");

            entity.Property(e => e.EstimatedEntryDate).HasColumnName("FECHA_ESTIMADA_INGRESO");
            entity.Property(e => e.State).HasColumnName("ESTADO");
            entity.Property(e => e.DeliverTo).HasColumnName("ENTREGAR_A");
            entity.Property(e => e.Observations)
                  .HasColumnName("OBSERVACIONES")
                  .HasColumnType("varchar(100)");

            entity.Property(e => e.ProviderNumber)
                  .HasColumnName("NUMERO_PROVEEDOR");

            entity.Property(e => e.Provider)
                  .HasColumnName("PROVEEDOR")
                  .HasColumnType("varchar(50)");

            entity.Property(e => e.ProviderOrderNumber)
                  .HasColumnName("NRO_PEDIDO_PROV")
                  .HasColumnType("varchar(10)");

            entity.Property(e => e.DeliveryDate)
                  .HasColumnName("FECHA_ENTREGA");

            entity.Property(e => e.IsRma)
                  .HasColumnName("ES_RMA")
                  .HasDefaultValue(0);

            entity.Property(e => e.Dollar).HasColumnName("DOLAR");
            entity.Property(e => e.TotalGoods).HasColumnName("TOTALMERC");
            entity.Property(e => e.Expenses).HasColumnName("GASTOS");
            entity.Property(e => e.TotalOrder).HasColumnName("TOTALPEDIDO");

            entity.Property(e => e.Paid)
                  .HasColumnName("PAGADO")
                  .HasDefaultValue((short)0);

            entity.Property(e => e.InvoiceId)
                  .HasColumnName("ID_FACTURA");

            entity.Property(e => e.BudgetItemId)
                  .HasColumnName("ID_ITEM_PRESUP")
                  .HasDefaultValue(-1);

            entity.Property(e => e.AuthorizedBy)
                  .HasColumnName("AUTORIZO")
                  .HasColumnType("varchar(20)");
        });
    }
}