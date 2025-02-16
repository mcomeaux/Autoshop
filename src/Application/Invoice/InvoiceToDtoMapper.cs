using Autoshop.Application.Common.Models;

namespace Autoshop.Application.Invoice
{
    public static class InvoiceToDtoMapper
    {
        public static InvoiceDto ToInvoiceDto(this Entities.Invoice invoice)
        {
            return new InvoiceDto{
                InvoiceId = invoice.InvoiceId,
                VisitId = invoice.VisitId,
                CustomerId = invoice.CustomerId,
                CreatedDate = invoice.CreatedDate,
                PaidOffDate = invoice.PaidOffDate,
                TotalCost = invoice.TotalCost,
                AmountPaid = invoice.AmountPaid,
                Description = invoice.Description
            };
        }
    }
}