using Autoshop.Application.Commands;
using MediatR;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;
using Autoshop.Application.Exceptions;

namespace Autoshop.Application.Visit
{
    public static class UpdateVisit
    {
        public class Command : IRequest<VisitDto>
        {
            public VisitDto UpdateRequest { get; set; }
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
                    VisitId = request.UpdateRequest.VisitId,
                    VehicleId = request.UpdateRequest.VehicleId,
                    DateOfArrival = request.UpdateRequest.DateOfArrival,
                    DateOfDeparture = request.UpdateRequest.DateOfDeparture,
                    Description = request.UpdateRequest.Description 
                });

                if(visit == null)
                {
                    throw new NotFoundException("Visit with Id [" + request.UpdateRequest.VisitId +  "] was not found.");
                }

                return visit.ToVisitDto();
            }
        }
    }
}