using MediatR;
using Autoshop.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Autoshop.Application.Queries
{
    public class GetVehiclesByCustomerIdQuery
    {
        public class Command : IRequest<List<Entities.Vehicle>>
        {
            public int CustomerId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, List<Entities.Vehicle>>
        {
            private readonly IApplicationDbContext _dbContext;


            public CommandHandler(IApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<List<Entities.Vehicle>> Handle(Command request, CancellationToken cancellationToken)
            {
                
                List<Entities.Vehicle> vehicles;

                vehicles = await _dbContext.Vehicles.Where(c => c.CustomerId == request.CustomerId)
                        .OrderBy(c => c.VehicleId)
                        .ToListAsync(cancellationToken: cancellationToken);

                return vehicles;
            }
        }
    }
}
