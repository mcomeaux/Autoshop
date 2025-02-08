using Autoshop.Application.Queries;
using MediatR;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace Autoshop.Application.Visit
{
    public static class GetActiveVisits
    {
        public class Command : IRequest<List<VisitDto>>
        {
            
        }

        public class CommandHandler : IRequestHandler<Command, List<VisitDto>>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(IMediator mediator, ILogger<CommandHandler> logger)
            {
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<List<VisitDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                List<VisitDto> result = new List<VisitDto>();

                //make db call
                List<Entities.Visit> visitList;

                visitList = await _mediator.Send(new GetActiveVisitsQuery.Command {  });

                foreach(var visit in visitList)
                {
                    result.Add(visit.ToVisitDto());
                }

                return result;
            }
        }
    }
}
