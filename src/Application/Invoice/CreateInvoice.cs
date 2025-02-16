using Autoshop.Application.Commands;
using MediatR;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace Autoshop.Application.Invoice
{
    public static class CreateInvoice
    {
        public class Command : IRequest<InvoiceDto>
        {
            public InvoiceDto CreateRequest { get; set; }
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
                    VisitId = request.CreateRequest.VisitId,
                    CustomerId = request.CreateRequest.CustomerId,
                    CreatedDate = request.CreateRequest.CreatedDate,
                    PaidOffDate = request.CreateRequest.PaidOffDate,
                    TotalCost = request.CreateRequest.TotalCost,
                    AmountPaid = request.CreateRequest.AmountPaid,
                    Description = request.CreateRequest.Description 
                });

                return invoice.ToInvoiceDto();
            }
        }
    }
}