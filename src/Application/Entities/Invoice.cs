
namespace Autoshop.Application.Entities
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int VisitId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? PaidOffDate { get; set; }
        public double TotalCost { get; set; }
        public double AmountPaid { get; set; } 
        public string Description { get; set; }

        public Visit Visit { get; set; }
        public Customer Customer { get; set; }
    }
}
