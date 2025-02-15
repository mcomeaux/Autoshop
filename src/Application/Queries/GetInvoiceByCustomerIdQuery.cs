using MediatR;
using Autoshop.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Autoshop.Application.Queries
{

    public class GetInvoicesByCustomerIdQuery
    {
        public class Command : IRequest<List<Entities.Invoice>>
        {
            public int CustomerId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, List<Entities.Invoice>>
        {
            private readonly IApplicationDbContext _dbContext;

            public CommandHandler(IApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<List<Entities.Invoice>> Handle(Command request, CancellationToken cancellationToken)
            {
                List<Entities.Invoice> invoices;

                invoices = await _dbContext.Invoices.Where(c => c.CustomerId == request.CustomerId)
                        .OrderBy(c => c.CreatedDate)
                        .ToListAsync(cancellationToken: cancellationToken);

                return invoices;
            }
        }
    }
}