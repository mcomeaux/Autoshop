using MediatR;
using Microsoft.EntityFrameworkCore;
using Autoshop.Application.Interfaces;
using Autoshop.Application.Entities;

namespace Autoshop.Application.Commands
{
    public class UpsertInvoiceCommand
    {
        public class Command : IRequest<Entities.Invoice>
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

        public class CommandHandler : IRequestHandler<Command, Entities.Invoice>
        {
            private readonly IApplicationDbContext _dbContext;

            public CommandHandler(IApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }
            public async Task<Entities.Invoice> Handle(Command request, CancellationToken cancellationToken)
            {
                //TODO: Don't allow insert of duplicate VisitIds
                //TODO: If VisitId does not exist return notfound exception
                Entities.Invoice invoice;
                if(request.InvoiceId != null)
                {
                    invoice = await _dbContext.Invoices.FirstOrDefaultAsync(v => v.InvoiceId == request.InvoiceId);
                    if(invoice == null){
                        throw(new Exception());//TODO: not found exception
                    }
                    invoice.VisitId = request.VisitId;
                    invoice.CustomerId = request.CustomerId;
                    invoice.CreatedDate = request.CreatedDate;
                    invoice.PaidOffDate = request.PaidOffDate;
                    invoice.TotalCost = request.TotalCost;
                    invoice.AmountPaid = request.AmountPaid;
                    invoice.Description = request.Description;
                }
                else{
                    var createdInvoice = await _dbContext.Invoices.AddAsync(new Entities.Invoice
                    {
                        VisitId = request.VisitId,
                        CustomerId = request.CustomerId,
                        CreatedDate = request.CreatedDate,
                        PaidOffDate = request.PaidOffDate,
                        TotalCost = request.TotalCost,
                        AmountPaid = request.AmountPaid,
                        Description = request.Description
                    });
                    invoice = createdInvoice.Entity;
                }
                

                try
                {
                    var result = await _dbContext.SaveChangesAsync(cancellationToken);

                    return invoice;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}