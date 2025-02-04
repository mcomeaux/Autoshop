using Autoshop.Application.Commands;
using MediatR;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;
using Autoshop.Application.Exceptions;

namespace Autoshop.Application.Customer
{
    public static class UpdateCustomer
    {
        public class Command : IRequest<GetCustomersResponse>
        {
            public CustomerRequest UpdateRequest { get; set; }
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
                    CustomerId = request.UpdateRequest.CustomerId, 
                    Name = request.UpdateRequest.Name,
                    Email = request.UpdateRequest.Email,
                    PhoneNumber = request.UpdateRequest.PhoneNumber,
                    Address = request.UpdateRequest.Address 
                });

                if(customer == null)
                {
                    throw new NotFoundException("Customer with Id [" + request.UpdateRequest.CustomerId +  "] was not found.");
                }

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