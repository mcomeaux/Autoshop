using MediatR;
using Autoshop.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Autoshop.Application.Queries
{
    public class GetVehicleByVehicleIdQuery
    {
        public class Command : IRequest<Entities.Vehicle>
        {
            public int VehicleId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Entities.Vehicle>
        {
            private readonly IApplicationDbContext _dbContext;


            public CommandHandler(IApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Entities.Vehicle> Handle(Command request, CancellationToken cancellationToken)
            {
                
                Entities.Vehicle vehicle;

                vehicle = await _dbContext.Vehicles
                    .FirstOrDefaultAsync(v => v.VehicleId == request.VehicleId);

                return vehicle;
            }
        }
    }
}
