using Autoshop.Application.Commands;
using MediatR;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace Autoshop.Application.Visit
{
    public static class CreateVisit
    {
        public class Command : IRequest<VisitDto>
        {
            public VisitDto CreateRequest { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, VisitDto>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(IMediator mediator, ILogger<CommandHandler> logger)
            {
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<VisitDto> Handle(Command request, CancellationToken cancellationToken)
            {
                Entities.Visit visit = await _mediator.Send(new UpsertVisitCommand.Command { 
                    VehicleId = request.CreateRequest.VehicleId,
                    DateOfArrival = request.CreateRequest.DateOfArrival,
                    DateOfDeparture = request.CreateRequest.DateOfDeparture,
                    Description = request.CreateRequest.Description 
                });

                return visit.ToVisitDto();
            }
        }
    }
}