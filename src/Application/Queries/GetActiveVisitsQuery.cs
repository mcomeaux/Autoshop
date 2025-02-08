using MediatR;
using Autoshop.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Autoshop.Application.Queries
{
    public class GetActiveVisitsQuery
    {
        public class Command : IRequest<List<Entities.Visit>>
        {
            
        }

        public class CommandHandler : IRequestHandler<Command, List<Entities.Visit>>
        {
            private readonly IApplicationDbContext _dbContext;


            public CommandHandler(IApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<List<Entities.Visit>> Handle(Command request, CancellationToken cancellationToken)
            {
                List<Entities.Visit> visits;

                visits = await _dbContext.Visits
                    .Where(c => c.DateOfDeparture == null)
                        .OrderBy(c => c.DateOfArrival)
                        .ToListAsync(cancellationToken: cancellationToken);

                return visits;
            }
        }
    }
}
