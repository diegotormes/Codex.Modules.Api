namespace Eternet.Purchasing.Api;

public class PurchasingContext(DbContextOptions options, ILoggerFactory? loggerFactory = null) : DbContext(options)
{
    public DbSet<Invoice> Invoices { get; set; } = null!;
    public DbSet<VatPurchase> VatPurchases { get; set; } = null!;
    public DbSet<InvoicePurchaseRetention> InvoicePurchaseRetentions { get; set; } = null!;
    public DbSet<PurchaseRetention> PurchaseRetentions { get; set; } = null!;
    public DbSet<InvoiceDetail> InvoiceDetails { get; set; } = null!;
    public DbSet<VendorOrder> VendorOrders { get; set; } = null!;
    public DbSet<CommercialRelationship> CommercialRelationships { get; set; } = null!;
    public DbSet<ExpenseType> ExpenseTypes { get; set; } = null!;
    public DbSet<Expense> Expenses { get; set; } = null!;
    public DbSet<VendorExpenseType> VendorExpenseTypes { get; set; } = null!;
    public DbSet<VendorBankAccount> VendorBankAccounts { get; set; } = null!;
    public DbSet<VendorCurrentAccount> VendorCurrentAccounts { get; set; } = null!;
    public DbSet<VendorServiceContract> VendorServiceContracts { get; set; } = null!;
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
    public DbSet<SalePoint> SalePoints { get; set; } = null!;
    public DbSet<ReceiptType> ReceiptTypes { get; set; } = null!;
    public DbSet<EarningTax> EarningTaxes { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (loggerFactory != null)
            optionsBuilder.UseLoggerFactory(loggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // FACTURAS_COMPRA
        modelBuilder.ConfigurePurchaseInvoice();
        // DETALLE_FC_COMPRA
        modelBuilder.ConfigurePurchaseInvoiceDetail();
        // FC_COMPRA_RETS
        modelBuilder.ConfigureInvoicePurchaseRetention();
        // IVA_COMPRA
        modelBuilder.ConfigureVatPurchase();
        // RETENCIONES_COMPRAS
        modelBuilder.ConfigurePurchaseRetention();
        // TIPOS_RETENCIONES
        modelBuilder.ConfigureRetentionType();
        // FC_COMPRA_FORMA_PAGO
        modelBuilder.ConfigurePurchasePaymentMethod();
        // CLIENTES
        modelBuilder.ConfigureCommercialRelationship();
        // PROVS_CTAS_BANCO
        modelBuilder.ConfigureVendorBankAccount();
        // CTASCTES_PROVS
        modelBuilder.ConfigureVendorCurrentAccount();
        // AFIP_GANANCIAS
        modelBuilder.ConfigureEarningTax();
        // PEDIDOS_PROV
        modelBuilder.ConfigureVendorOrder();
        // GASTOS
        modelBuilder.ConfigureExpense();
        // PROVS_TIPOS_GASTOS
        modelBuilder.ConfigureVendorExpenseType();
        // TIPOS_GASTOS
        modelBuilder.ConfigureExpenseType();
        // PROVS_SERVS_CONT
        modelBuilder.ConfigureVendorServiceContract();
        // FORMAS_PAGO
        modelBuilder.ConfigurePaymentMethod();
        // PROVINCIAS
        modelBuilder.ConfigureProvince();
        // LOCALIDADES
        modelBuilder.ConfigureCity();
        // PUNTOS_VENTA
        modelBuilder.ConfigureSalePoint();
        // TIPOS_COMPROBANTE
        modelBuilder.ConfigureReceiptType();
    }
}