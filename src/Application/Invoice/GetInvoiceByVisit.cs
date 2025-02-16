using Autoshop.Application.Queries;
using MediatR;
using Autoshop.Application.Exceptions;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace Autoshop.Application.Invoice
{
    public static class GetInvoiceByVisit
    {
        public class Command : IRequest<InvoiceDto>
        {
            public int VisitId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, InvoiceDto>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(IMediator mediator, ILogger<CommandHandler> logger)
            {
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<InvoiceDto> Handle(Command request, CancellationToken cancellationToken)
            {
                //make db call
                Entities.Invoice invoice;

                invoice = await _mediator.Send(new GetInvoiceByVisitIdQuery.Command { VisitId = request.VisitId });
                if(invoice == null)
                {
                    throw new NotFoundException("Invoice with VisitId [" + request.VisitId +  "] was not found.");
                }

                return invoice.ToInvoiceDto();
            }
        }
    }
}
