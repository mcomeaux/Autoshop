using Autoshop.Application.Commands;
using MediatR;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace Autoshop.Application.Customer
{
    public static class CreateCustomer
    {
        public class Command : IRequest<GetCustomersResponse>
        {
            public CustomerRequest CreateRequest { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, GetCustomersResponse>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(IMediator mediator, ILogger<CommandHandler> logger)
            {
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<GetCustomersResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                GetCustomersResponse result = new GetCustomersResponse();

                Entities.Customer customer = await _mediator.Send(new UpsertCustomerCommand.Command { 
                    Name = request.CreateRequest.Name,
                    Email = request.CreateRequest.Email,
                    PhoneNumber = request.CreateRequest.PhoneNumber,
                    Address = request.CreateRequest.Address 
                });

                //TODO: Use Mapper here

                result.CustomerId = customer.CustomerId;
                result.Name = customer.Name;
                result.Email = customer.Email;
                result.Address = customer.Address;
                result.PhoneNumber = customer.PhoneNumber;

                return result;
            }
        }
    }
}