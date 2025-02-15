using MediatR;
using Autoshop.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Autoshop.Application.Queries
{

    public class GetInvoiceByVisitIdQuery
    {
        public class Command : IRequest<Entities.Invoice>
        {
            public int VisitId { get; set; }
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
                var result = await _dbContext.Invoices
                    .FirstOrDefaultAsync(c => c.VisitId == request.VisitId);


                return result;
            }
        }
    }
}