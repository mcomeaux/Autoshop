using Autoshop.Application.Queries;
using MediatR;
using Autoshop.Application.Exceptions;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace Autoshop.Application.Customer
{
    public static class GetCustomer
    {
        public class Command : IRequest<GetCustomersResponse>
        {
            public int CustomerId { get; set; }
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

                //make db call
                Entities.Customer customer;

                customer = await _mediator.Send(new GetCustomerByIdQuery.Command { CustomerId = request.CustomerId });
                if(customer == null)
                {
                    throw new NotFoundException("Customer " + request.CustomerId +  " was not found");
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
