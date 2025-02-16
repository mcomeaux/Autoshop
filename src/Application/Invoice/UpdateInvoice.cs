using Autoshop.Application.Commands;
using MediatR;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;
using Autoshop.Application.Exceptions;

namespace Autoshop.Application.Invoice
{
    public static class UpdateInvoice
    {
        public class Command : IRequest<InvoiceDto>
        {
            public InvoiceDto UpdateRequest { get; set; }
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
                Entities.Invoice invoice = await _mediator.Send(new UpsertInvoiceCommand.Command { 
                    InvoiceId = request.UpdateRequest.InvoiceId,
                    VisitId = request.UpdateRequest.VisitId,
                    CustomerId = request.UpdateRequest.CustomerId,
                    CreatedDate = request.UpdateRequest.CreatedDate,
                    PaidOffDate = request.UpdateRequest.PaidOffDate,
                    TotalCost = request.UpdateRequest.TotalCost,
                    AmountPaid = request.UpdateRequest.AmountPaid,
                    Description = request.UpdateRequest.Description 
                });

                if(invoice == null)
                {
                    throw new NotFoundException("Invoice with Id [" + request.UpdateRequest.InvoiceId +  "] was not found.");
                }

                return invoice.ToInvoiceDto();
            }
        }
    }
}