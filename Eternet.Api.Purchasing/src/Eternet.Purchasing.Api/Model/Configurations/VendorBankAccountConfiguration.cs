using Eternet.Purchasing.Api.Converters;

namespace Eternet.Purchasing.Api.Model.Configurations;

public static class VendorBankAccountConfiguration
{
    public static void ConfigureVendorBankAccount(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VendorBankAccount>(entity =>
        {
            entity.ToTable("PROVS_CTAS_BANCO");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CommercialRelationshipId).HasColumnName("ID_PROVEEDOR");
            entity.Property(e => e.Local)
                .HasColumnName("CUENTA_LOCAL")
                .HasConversion(
                    v => EFShortToBoolConverterExtensions.ToLocalDbModel(v),
                    v => EFShortToBoolConverterExtensions.ToLocalDomainModel(v));
            entity.Property(e => e.Type).HasColumnName("TIPO_CUENTA");
            entity.Property(e => e.Branch).HasColumnName("SUCURSAL");
            entity.Property(e => e.Number).HasColumnName("NRO_CUENTA");
            entity.Property(e => e.CbuBlockOne).HasColumnName("CBU1");
            entity.Property(e => e.CbuBlockTwo).HasColumnName("CBU2");
            entity.Property(e => e.DniType).HasColumnName("TIPO_DOC");
            entity.Property(e => e.Beneficiary).HasColumnName("BENEFICIARIO");
            entity.Property(e => e.Email).HasColumnName("MAIL");
            entity.Property(e => e.Dni).HasColumnName("NRO_DOC");
            entity.Property(e => e.Enabled).HasColumnName("HABILITADA");
        });
    }
}