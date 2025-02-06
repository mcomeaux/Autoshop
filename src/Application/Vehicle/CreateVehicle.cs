using Autoshop.Application.Commands;
using MediatR;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace Autoshop.Application.Vehicle
{
    public static class CreateVehicle
    {
        public class Command : IRequest<VehicleDto>
        {
            public VehicleDto CreateRequest { get; set; }
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
                    CustomerId = request.CreateRequest.CustomerId,
                    Make = request.CreateRequest.Make,
                    Model = request.CreateRequest.Model,
                    Year = request.CreateRequest.Year,
                    Color = request.CreateRequest.Color,
                    VIN = request.CreateRequest.VIN,
                    Description = request.CreateRequest.Description 
                });

                return vehicle.ToVehicleDto();
            }
        }
    }
}