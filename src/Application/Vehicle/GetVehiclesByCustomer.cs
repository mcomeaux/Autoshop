using Autoshop.Application.Queries;
using MediatR;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace Autoshop.Application.Vehicle
{
    public static class GetVehiclesByCustomer
    {
        public class Command : IRequest<List<VehicleDto>>
        {
            public int CustomerId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, List<VehicleDto>>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(IMediator mediator, ILogger<CommandHandler> logger)
            {
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<List<VehicleDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                List<VehicleDto> result = new List<VehicleDto>();

                //make db call
                List<Entities.Vehicle> vehicleList;

                vehicleList = await _mediator.Send(new GetVehiclesByCustomerIdQuery.Command { CustomerId = request.CustomerId });

                foreach(var vehicle in vehicleList)
                {
                    result.Add(vehicle.ToVehicleDto());
                }

                return result;
            }
        }
    }
}
