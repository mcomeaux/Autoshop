using Autoshop.Application.Queries;
using MediatR;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace Autoshop.Application.Vehicle
{
    public static class GetVehiclesByCustomer
    {
        public class Command : IRequest<List<GetVehiclesResponse>>
        {
            public int CustomerId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, List<GetVehiclesResponse>>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(IMediator mediator, ILogger<CommandHandler> logger)
            {
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<List<GetVehiclesResponse>> Handle(Command request, CancellationToken cancellationToken)
            {
                List<GetVehiclesResponse> result = new List<GetVehiclesResponse>();

                //make db call
                List<Entities.Vehicle> vehicleList;

                vehicleList = await _mediator.Send(new GetVehiclesByCustomerIdQuery.Command { CustomerId = request.CustomerId });

                //TODO: Use Mapper here

                foreach(var vehicle in vehicleList)
                {
                    result.Add(new GetVehiclesResponse {
                        VehicleId = vehicle.VehicleId,
                        CustomerId = vehicle.CustomerId,
                        Make = vehicle.Make,
                        Model = vehicle.Model,
                        Year = vehicle.Year,
                        Color = vehicle.Color,
                        Description = vehicle.Description
                    });
                }

                return result;
            }
        }
    }
}
