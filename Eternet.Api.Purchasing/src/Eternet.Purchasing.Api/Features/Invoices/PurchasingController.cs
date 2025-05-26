using Eternet.Purchasing.Api.Extensions;
using Eternet.Purchasing.Contracts.Invoices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Eternet.Purchasing.Api.Features.Invoices;

[Route(Config.ControllerName)]
public class PurchasingController(PurchasingContext dbContext) : ODataController
{
    [EnableQuery(PageSize = 100, EnsureStableOrdering = false)]
    [HttpGet(Config.Invoices)]
    public IActionResult GetInvoices()
    {
        var hasOrderBy = Request.Query.ContainsKey("$orderby");
        var query = dbContext.Invoices.Select(InvoiceMappingExpressions.ToInvoiceResponseExpression());
        if (!hasOrderBy)
        {
            query = query.OrderByDescending(x => x.InternalId);
        }
        return Ok(query);
    }

    [EnableQuery]
    [HttpGet($"{Config.Invoices}/{{id}}")]
    public SingleResult<InvoiceResponse> GetInvoice(Guid id)
    {
        return SingleResult
            .Create(dbContext.Invoices.Where(i => i.Guid == id)
            .Select(InvoiceMappingExpressions.ToInvoiceResponseExpression()));
    }
}