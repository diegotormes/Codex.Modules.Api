namespace Eternet.Purchasing.Api.Model.Configurations;

public static class CommercialRelationshipConfiguration
{
    public static void ConfigureCommercialRelationship(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CommercialRelationship>(entity =>
        {
            entity.ToTable("CLIENTES");

            // Primary key
            entity.HasKey(e => e.Id)
                  .HasName("PK_CLIENTES");

            entity.Property(e => e.Id)
                  .HasColumnName("CLAVE");

            // Basic fields
            entity.Property(e => e.DocumentType)
                  .HasColumnName("TIPO_DOCUMENTO")
                  .HasColumnType("varchar(4)")
                  .HasMaxLength(4);

            entity.Property(e => e.DocumentNumber)
                  .HasColumnName("NRO_DOCUMENTO")
                  .HasColumnType("varchar(10)")
                  .HasMaxLength(10);

            entity.Property(e => e.SurnameAndName)
                  .HasColumnName("AYN")
                  .HasColumnType("varchar(40)")
                  .HasMaxLength(40)
                  .IsRequired();

            entity.Property(e => e.PhoneNumber)
                  .HasColumnName("TELEFONO")
                  .HasColumnType("varchar(30)")
                  .HasMaxLength(30);

            entity.Property(e => e.VatStatus)
                  .HasColumnName("IVA")
                  .HasColumnType("varchar(3)")
                  .HasMaxLength(3)
                  .IsRequired();

            entity.Property(e => e.VendorTaxId)
                  .HasColumnName("CUIT")
                  .HasColumnType("varchar(13)")
                  .HasMaxLength(13);

            entity.Property(e => e.SalesPoint)
                  .HasColumnName("PUNTO_VENTA");

            entity.Property(e => e.RelationshipType)
                  .HasColumnName("TIPO_RELACION")
                  .HasColumnType("varchar(17)")
                  .HasMaxLength(17)
                  .HasDefaultValue("Cliente")
                  .IsRequired();

            entity.Property(e => e.Observations)
                  .HasColumnName("OBSERVACIONES")
                  .HasColumnType("BLOB SUB_TYPE TEXT")
                  .IsUnicode(false);

            entity.Property(e => e.MonthlyBilling)
                  .HasColumnName("FACTURACION_MENSUAL")
                  .HasColumnType("varchar(2)")
                  .HasMaxLength(2)
                  .HasDefaultValue("No");

            entity.Property(e => e.PostalAddress)
                  .HasColumnName("DOMICILIO")
                  .HasColumnType("varchar(35)")
                  .HasMaxLength(35);

            entity.Property(e => e.PaymentAddress)
                  .HasColumnName("DOMICILIO_PAG0")
                  .HasColumnType("varchar(35)")
                  .HasMaxLength(35);

            entity.Property(e => e.PostalCode)
                  .HasColumnName("CODIGO_POSTAL")
                  .HasColumnType("varchar(12)")
                  .HasMaxLength(12);

            entity.Property(e => e.CityDescription)
                  .HasColumnName("LOCALIDAD")
                  .HasColumnType("varchar(35)")
                  .HasMaxLength(35);

            entity.Property(e => e.Balance)
                  .HasColumnName("SALDO")
                  .HasDefaultValue(0);

            entity.Property(e => e.Uncollectible)
                  .HasColumnName("INCOBRABLE")
                  .HasColumnType("varchar(2)")
                  .HasMaxLength(2)
                  .HasDefaultValue("No");

            entity.Property(e => e.Debit)
                  .HasColumnName("DEBITO")
                  .HasColumnType("varchar(2)")
                  .HasMaxLength(2)
                  .HasDefaultValue("No");

            entity.Property(e => e.Area)
                  .HasColumnName("ZONA")
                  .HasDefaultValue((short)0);

            entity.Property(e => e.Category)
                  .HasColumnName("CATEGORIA")
                  .HasDefaultValue(1);

            entity.Property(e => e.ProvBalance)
                  .HasColumnName("SALDO_PROV")
                  .HasDefaultValue(0);

            entity.Property(e => e.Street)
                  .HasColumnName("CALLE");

            entity.Property(e => e.CityId)
                  .HasColumnName("IDLOCALIDAD");

            entity.Property(e => e.StreetNumber)
                  .HasColumnName("ALTURA");

            entity.Property(e => e.PaymentStreet)
                  .HasColumnName("CALLE_PAGO");

            entity.Property(e => e.PaymentStreetHeight)
                  .HasColumnName("ALTURA_PAGO");

            entity.Property(e => e.PhoneAreaCode)
                  .HasColumnName("AREA_TELEFONICA")
                  .HasColumnType("varchar(15)")
                  .HasMaxLength(15);

            entity.Property(e => e.BaproCouponNumber)
                  .HasColumnName("NRO_CUPON_BAPRO")
                  .HasDefaultValue(1);

            entity.Property(e => e.PaymentPostalCode)
                  .HasColumnName("CODIGO_POSTAL_PAGO")
                  .HasColumnType("varchar(12)")
                  .HasMaxLength(12);

            entity.Property(e => e.HasTelephony)
                  .HasColumnName("TIENE_TELEFONIA")
                  .HasDefaultValue((short)0);

            entity.Property(e => e.DebitType)
                  .HasColumnName("TIPO_DEBITO")
                  .HasDefaultValue((short)0);

            entity.Property(e => e.CardholderName)
                  .HasColumnName("TITULAR_TARJETA")
                  .HasColumnType("varchar(40)")
                  .HasMaxLength(40);

            entity.Property(e => e.DebtMessage)
                  .HasColumnName("MENSAJE_DEUDA")
                  .HasDefaultValue((short)1);

            entity.Property(e => e.Neighborhood)
                  .HasColumnName("BARRIO")
                  .HasDefaultValue(0);

            entity.Property(e => e.DeliveryCity)
                  .HasColumnName("LOCALIDAD_REPARTO");

            entity.Property(e => e.PrintedBill)
                  .HasColumnName("FACTURA_IMPRESA")
                  .HasDefaultValue((short)0);

            entity.Property(e => e.SmsCellphone)
                  .HasColumnName("CELULAR_SMS")
                  .HasColumnType("varchar(10)")
                  .HasMaxLength(10);

            entity.Property(e => e.DoesNotInformMail)
                  .HasColumnName("NO_INFORMA_MAIL")
                  .HasDefaultValue((short)0);

            entity.Property(e => e.InvalidMail)
                  .HasColumnName("MAIL_INVALIDO")
                  .HasDefaultValue((short)0);

            entity.Property(e => e.AccountingChartId)
                  .HasColumnName("ID_CUENTA_PP");

            entity.Property(e => e.FirstName)
                  .HasColumnName("NOMBRE_PILA")
                  .HasColumnType("varchar(30)")
                  .HasMaxLength(30);

            entity.Property(e => e.AppliesEarningsTax)
                  .HasColumnName("APLICA_GANANCIAS");

            entity.Property(e => e.IvaBook)
                  .HasColumnName("LIBRO_IVA")
                  .HasDefaultValue((short)1);

            entity.Property(e => e.HighValue)
                  .HasColumnName("ALTO_VALOR")
                  .HasDefaultValue((short)0);

            entity.Property(e => e.DgoActive)
                  .HasColumnName("DGO_ACTIVO")
                  .HasDefaultValue((short)0);

            entity.Property(e => e.LastDgoUser)
                  .HasColumnName("ULT_USUARIO_DGO")
                  .HasColumnType("varchar(20)")
                  .HasMaxLength(20);

            entity.Property(e => e.LastDgoDate)
                  .HasColumnName("ULT_FECHA_DGO");

            entity.Property(e => e.PaymentMethodId)
                  .HasColumnName("FORMA_PAGO_PROV");

            entity.Property(e => e.PaymentTermsInDays)
                  .HasColumnName("PLAZO_PAGO_PROV");

            entity.HasOne(cr => cr.PaymentMethod)
                  .WithMany()
                  .HasForeignKey(cr => cr.PaymentMethodId)
                  .HasConstraintName("FK_CLIENTES_FPAGO_PROV")
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(cr => cr.PurchaseInvoices)
                  .WithOne(pi => pi.CommercialRelationship)
                  .HasForeignKey(pi => pi.VendorId)
                  .HasConstraintName("PK_FACTURAS_COMPRA_CLIENTES_FB")
                  .OnDelete(DeleteBehavior.ClientNoAction);
        });
    }
}