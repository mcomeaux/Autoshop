using Autoshop.Application.Commands;
using MediatR;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace Autoshop.Application.Customer
{
    public static class CreateCustomer
    {
        public class Command : IRequest<CustomerDto>
        {
            public CustomerDto CreateRequest { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, CustomerDto>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(IMediator mediator, ILogger<CommandHandler> logger)
            {
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<CustomerDto> Handle(Command request, CancellationToken cancellationToken)
            {
                Entities.Customer customer = await _mediator.Send(new UpsertCustomerCommand.Command { 
                    Name = request.CreateRequest.Name,
                    Email = request.CreateRequest.Email,
                    PhoneNumber = request.CreateRequest.PhoneNumber,
                    Address = request.CreateRequest.Address 
                });

                return customer.ToCustomerDto();
            }
        }
    }
}