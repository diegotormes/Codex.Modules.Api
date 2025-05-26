namespace Eternet.Accounting.Api.Features.VatClosures.Close.Steps;

public class VatDebitItem
{
    public decimal? VatAmount { get; set; }
}

public class VatCreditItem
{
    public decimal? VatAmount { get; set; }
}

public class VatRetentionItem
{
    public decimal? RetentionAmount { get; set; }
}

public class VatSalaryItem
{
    public int Id { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? FinalConsumerAmount { get; set; }
}
