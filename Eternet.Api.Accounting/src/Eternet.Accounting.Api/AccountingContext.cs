namespace Eternet.Accounting.Api;

public class AccountingContext : DbContext
{
    private ILoggerFactory? _loggerFactory;

    public AccountingContext(
        DbContextOptions<AccountingContext> options,
        ILoggerFactory? loggerFactory = null)
        : base(options)
    {
        _loggerFactory = loggerFactory;
    }

    public DbSet<GeneralLedgerAccount> GeneralLedgerAccounts { get; set; } = null!;
    public DbSet<JournalEntry> JournalEntries { get; set; } = null!;
    public DbSet<JournalEntryDetail> JournalEntryDetails { get; set; } = null!;
    public DbSet<AccountingEntryRecord> AccountingEntryRecords { get; set; } = null!;
    public DbSet<AccountingEntryRecordDetail> AccountingEntryRecordDetails { get; set; } = null!;
    public DbSet<AccountingPeriod> AccountingPeriods { get; set; } = null!;
    public DbSet<AccountingReference> AccountingReferences { get; set; } = null!;
    public DbSet<VatSalaryClosureDetail> VatSalaryClosureDetails { get; set; } = null!;
    public DbSet<SalaryClosure> SalaryClosures { get; set; } = null!;
    public DbSet<SalaryClosureDetail> SalaryClosureDetails { get; set; } = null!;
    public DbSet<SalaryItem> SalaryItems { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<CustomerAccount> CustomerAccounts { get; set; } = null!;
    public DbSet<RetentionSale> SalesRetentions { get; set; } = null!;
    public DbSet<RetentionPurchase> PurchaseRetentions { get; set; } = null!;
    public DbSet<VatPurchase> VatPurchases { get; set; } = null!;
    public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; } = null!;
    public DbSet<PurchaseRetentionDetail> PurchaseRetentionDetails { get; set; } = null!;
    public DbSet<RetentionType> RetentionTypes { get; set; } = null!;
    public DbSet<SalesInvoice> SalesInvoices { get; set; } = null!;
    public DbSet<SalesInvoiceDetail> SalesInvoiceDetails { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (_loggerFactory != null)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountingContext).Assembly);
        if (Database.ProviderName?.Contains("Sqlite", StringComparison.OrdinalIgnoreCase) == true)
        {
            modelBuilder.Entity<Model.JournalEntry>()
                .Property(e => e.Guid)
                .HasColumnType("BLOB");
        }
        // Register query types for raw SQL queries
        modelBuilder.Entity<Features.VatClosures.Close.Steps.VatDebitItem>().HasNoKey();
        modelBuilder.Entity<Features.VatClosures.Close.Steps.VatCreditItem>().HasNoKey();
        modelBuilder.Entity<Features.VatClosures.Close.Steps.VatRetentionItem>().HasNoKey();
        modelBuilder.Entity<Features.VatClosures.Close.Steps.VatSalaryItem>().HasNoKey();
        modelBuilder.Entity<Features.VatClosures.Preview.VatDebitEntry>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasColumnName("DESCRIPCION");
            entity.Property(e => e.TaxResponsibility).HasColumnName("RESP_IVA");
            entity.Property(e => e.VatRate).HasColumnName("TASA_IVA");
            entity.Property(e => e.TaxableAmount).HasColumnName("IMPONIBLE");
            entity.Property(e => e.VatAmount).HasColumnName("IVA");
            entity.Property(e => e.Total).HasColumnName("TOTAL");
        });

        modelBuilder.Entity<Features.VatClosures.Preview.VatCreditEntry>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasColumnName("DESCRIPCION");
            entity.Property(e => e.VatRate).HasColumnName("TASA_IVA");
            entity.Property(e => e.TaxableAmount).HasColumnName("IMPONIBLE");
            entity.Property(e => e.VatAmount).HasColumnName("IVA");
            entity.Property(e => e.Total).HasColumnName("TOTAL");
        });

        modelBuilder.Entity<Features.VatClosures.Preview.VatRetentionEntry>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnName("FECHA");
            entity.Property(e => e.RetentionType).HasColumnName("TIPO_RETENCION");
            entity.Property(e => e.Amount).HasColumnName("IMPORTE");
            entity.Property(e => e.Name).HasColumnName("RAZON_SOCIAL");
            entity.Property(e => e.TaxNumber).HasColumnName("CUIT");
            entity.Property(e => e.RetentionCode).HasColumnName("CODIGO_RETENCION");
            entity.Property(e => e.CertificateNumber).HasColumnName("NRO_CERTIFICADO");
        });

        modelBuilder.Entity<Features.VatClosures.Preview.VatSalaryEntry>(entity =>
        {
            entity.HasNoKey();
            entity.Property(e => e.Month).HasColumnName("MES");
            entity.Property(e => e.Year).HasColumnName("ANIO");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasColumnName("DESCRIPCION");
            entity.Property(e => e.IvaCf).HasColumnName("CF");
            entity.Property(e => e.Total).HasColumnName("TOTAL");
        });
        base.OnModelCreating(modelBuilder);
    }
}
