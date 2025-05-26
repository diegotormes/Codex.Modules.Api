namespace Eternet.Purchasing.Api.Model;

public static class PurchaseInvoiceConstraints
{
    public static int DescriptionMaxLen { get; } = 45;
    public static int VendorNameMaxLen { get; } = 40;
    public static int BarcodeMaxLen { get; } = 40;
    public static int CuitMaxLen { get; } = 13;
    public static int VendorPostalAddressMaxLen { get; } = 35;
    public static int InvoiceTypeMaxLen { get; } = 1;
    public static int VoucherTypeMaxLen { get; } = 2;
    public static int BranchNumberMaxLen { get; } = 4;
    public static int VoucherNumberMaxLen { get; } = 8;
    public static int SharePointFileMaxLen { get; set; } = 180;
}
