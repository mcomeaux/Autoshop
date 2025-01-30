using MediatR;
using Autoshop.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Autoshop.Application.Queries
{
    public class GetCustomersQuery
    {
        public class Command : IRequest<List<Entities.Customer>>
        {
            public string Name { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, List<Entities.Customer>>
        {
            private readonly IApplicationDbContext _dbContext;


            public CommandHandler(IApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<List<Entities.Customer>> Handle(Command request, CancellationToken cancellationToken)
            {
                
                List<Entities.Customer> customers;

                customers = await _dbContext.Customers.Where(c => c.Name.ToLowerInvariant().Contains(request.Name.ToLowerInvariant()))
                        .OrderBy(c => c.Name)
                        .ToListAsync();

                return customers;
            }
        }
    }
}
