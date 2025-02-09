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
            public string PhoneNumber { get; set; }
            public int? Page { get; set; }
            public int? PageSize { get; set; }
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

                var pn = (request.Page.HasValue && request.Page.Value > 0)
                ? request.Page.Value
                : 1;

                var ps = (request.PageSize.HasValue && request.PageSize.Value > 0)
                ? request.PageSize.Value
                : 25;

                customers = await _dbContext.Customers
                    .Where(c => c.Name.ToLower().Contains(request.Name.ToLower()))
                    .Where(c => request.PhoneNumber == "" ? true : c.PhoneNumber == request.PhoneNumber)
                    .OrderBy(c => c.Name)
                    .Skip((pn - 1) * ps)
                    .Take(ps)
                    .ToListAsync(cancellationToken: cancellationToken);

                return customers;
            }
        }
    }
}
