using Autoshop.Application.Commands;
using MediatR;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;
using Autoshop.Application.Exceptions;

namespace Autoshop.Application.Vehicle
{
    public static class UpdateVehicle
    {
        public class Command : IRequest<VehicleDto>
        {
            public VehicleDto UpdateRequest { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, VehicleDto>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(IMediator mediator, ILogger<CommandHandler> logger)
            {
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<VehicleDto> Handle(Command request, CancellationToken cancellationToken)
            {
                Entities.Vehicle vehicle = await _mediator.Send(new UpsertVehicleCommand.Command { 
                    VehicleId = request.UpdateRequest.VehicleId,
                    CustomerId = request.UpdateRequest.CustomerId,
                    Make = request.UpdateRequest.Make,
                    Model = request.UpdateRequest.Model,
                    Year = request.UpdateRequest.Year,
                    Color = request.UpdateRequest.Color,
                    VIN = request.UpdateRequest.VIN,
                    Description = request.UpdateRequest.Description 
                });

                if(vehicle == null)
                {
                    throw new NotFoundException("Vehicle with Id [" + request.UpdateRequest.VehicleId +  "] was not found.");
                }

                return vehicle.ToVehicleDto();
            }
        }
    }
}