using MediatR;
using Autoshop.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Autoshop.Application.Queries
{

    public class GetCustomerByCustomerIdQuery
    {
        public class Command : IRequest<Entities.Customer>
        {
            public int CustomerId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Entities.Customer>
        {
            private readonly IApplicationDbContext _dbContext;

            public CommandHandler(IApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Entities.Customer> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await _dbContext.Customers
                    .FirstOrDefaultAsync(c => c.CustomerId == request.CustomerId);


                return result;
            }
        }
    }
}