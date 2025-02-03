using MediatR;
using Microsoft.EntityFrameworkCore;
using Autoshop.Application.Interfaces;
using Autoshop.Application.Entities;

namespace Autoshop.Application.Commands
{
    public class UpsertVehicleCommand
    {
        public class Command : IRequest<Entities.Vehicle>
        {
            public int? VehicleId { get; set; }
            public int CustomerId { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
            public string Color { get; set; }
            public string Description { get; set; }
            public int Year { get; set; }
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
                if(request.VehicleId != null)
                {
                    vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == request.VehicleId);
                    if(vehicle == null){
                        throw(new Exception());//TODO: not found exception
                    }
                    vehicle.CustomerId = request.CustomerId;
                    vehicle.Make = request.Make;
                    vehicle.Model = request.Model;
                    vehicle.Year = request.Year;
                    vehicle.Color = request.Color;
                    vehicle.Description = request.Description;
                }
                else{
                    var createdVehicle = await _dbContext.Vehicles.AddAsync(new Entities.Vehicle
                    {
                        CustomerId = request.CustomerId,
                        Make = request.Make,
                        Model = request.Model,
                        Year = request.Year,
                        Color = request.Color,
                        Description = request.Description,
                    });
                    vehicle = createdVehicle.Entity;
                }
                

                try
                {
                    var result = await _dbContext.SaveChangesAsync(cancellationToken);

                    return vehicle;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}