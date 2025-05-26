
namespace Eternet.Purchasing.Api.Features.DebtInstruments.Create.Steps;

public record VendorValidated(int Id, string Name, string VatStatus, int PaymentMethodId);

public class ValidateVendorExistsAsync(PurchasingContext dbContext) : BaseHandlerResponses
{
    public async Task<StepResult<VendorValidated>> Handle(
        string vendorTaxId,
        CancellationToken cancellationToken)
    {
        var vendor = await dbContext.CommercialRelationships
            .AsNoTracking()
            .Where(v => v.VendorTaxId == vendorTaxId)
            .Include(v=> v.PaymentMethod)
            .Select(v => new {v.Id, v.SurnameAndName, v.RelationshipType, v.VatStatus, v.PaymentMethod })
            .ToListAsync(cancellationToken);

        if (vendor.Count == 0)
        {
            return InvalidStateError(GetNotFoundError(vendorTaxId));
        }

        var validatedVendors = vendor.Where(cr => cr.RelationshipType.Contains("Prov")).ToArray();
        if (validatedVendors.Length > 1)
        {
            return InvalidStateError(GetMultipleVendorsError(validatedVendors.Length, vendorTaxId));
        }

        return new VendorValidated(
            validatedVendors[0].Id, 
            validatedVendors[0].SurnameAndName, 
            validatedVendors[0].VatStatus,
            validatedVendors[0].PaymentMethod?.Id ?? 5);
    }

    public static string GetNotFoundError(string vendorTaxId)
    {
        return $"Vendor with Id {vendorTaxId} not found";
    }

    public static string GetMultipleVendorsError(int vendorCount, string vendorTaxId)
    {
        return $"Multiple vendors found ({vendorCount}) with Id {vendorTaxId}";
    }
}