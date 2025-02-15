namespace Autoshop.Application.Common.Models
{
    public class InvoiceDto
    {
        public int? InvoiceId { get; set; }
        public int VisitId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? PaidOffDate { get; set; }
        public double TotalCost { get; set; }
        public double AmountPaid { get; set; } 
        public string Description { get; set; }
    }
}