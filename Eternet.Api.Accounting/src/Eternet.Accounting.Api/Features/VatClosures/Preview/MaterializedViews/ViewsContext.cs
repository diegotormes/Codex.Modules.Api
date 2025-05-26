namespace Eternet.Accounting.Api.Features.VatClosures.Preview.MaterializedViews;

public class ViewsContext(DbContextOptions<ViewsContext> options) : DbContext(options)
{
    public DbSet<VatDebitEntry> VatDebits { get; set; } = null!;
    public DbSet<VatCreditEntry> VatCredits { get; set; } = null!;
    public DbSet<VatRetentionEntry> VatRetentions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VatDebitEntry>().HasKey(e => e.IdInternal);
        modelBuilder.Entity<VatDebitEntry>().Property(e => e.IdInternal).ValueGeneratedOnAdd();
        modelBuilder.Entity<VatDebitEntry>().HasIndex(e => e.VatRate);
        modelBuilder.Entity<VatCreditEntry>().HasKey(e => e.IdInternal);
        modelBuilder.Entity<VatCreditEntry>().Property(e => e.IdInternal).ValueGeneratedOnAdd();
        modelBuilder.Entity<VatCreditEntry>().HasIndex(e => e.VatRate);
        modelBuilder.Entity<VatRetentionEntry>().HasKey(e => e.IdInternal);
        modelBuilder.Entity<VatRetentionEntry>().Property(e => e.IdInternal).ValueGeneratedOnAdd();
        modelBuilder.Entity<VatRetentionEntry>().HasIndex(e => e.Date);
    }
}
