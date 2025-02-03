using Autoshop.Application.Commands;
using MediatR;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace Autoshop.Application.Vehicle
{
    public static class CreateVehicle
    {
        public class Command : IRequest<GetVehiclesResponse>
        {
            public VehicleRequest CreateRequest { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, GetVehiclesResponse>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(IMediator mediator, ILogger<CommandHandler> logger)
            {
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<GetVehiclesResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                GetVehiclesResponse result = new GetVehiclesResponse();

                Entities.Vehicle vehicle = await _mediator.Send(new UpsertVehicleCommand.Command { 
                    CustomerId = request.CreateRequest.CustomerId,
                    Make = request.CreateRequest.Make,
                    Model = request.CreateRequest.Model,
                    Year = request.CreateRequest.Year,
                    Color = request.CreateRequest.Color,
                    Description = request.CreateRequest.Description 
                });

                //TODO: Use Mapper here

                result.VehicleId = vehicle.VehicleId;
                result.CustomerId = vehicle.CustomerId;
                result.Make = vehicle.Make;
                result.Model = vehicle.Model;
                result.Year = vehicle.Year;
                result.Color = vehicle.Color;
                result.Description = vehicle.Description;

                return result;
            }
        }
    }
}