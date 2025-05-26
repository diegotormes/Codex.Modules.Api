namespace Eternet.Purchasing.Api.Model.Configurations;

public static class PaymentMethodConfiguration
{
    public static void ConfigurePaymentMethod(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.ToTable("FORMAS_PAGO");
            entity.HasKey(p => p.Id);

            entity.Property(e => e.Id)
                  .HasColumnName("ID");

            entity.Property(e => e.Description)
                  .HasColumnName("DESCRIPCION")
                  .HasMaxLength(PaymentMethodConstraints.DescriptionMaxLen)
                  .HasColumnType($"varchar({PaymentMethodConstraints.DescriptionMaxLen})");
        });
    }
}