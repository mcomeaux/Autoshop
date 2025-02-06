using Autoshop.Application.Queries;
using MediatR;
using Autoshop.Application.Exceptions;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace Autoshop.Application.Customer
{
    public static class GetCustomer
    {
        public class Command : IRequest<CustomerDto>
        {
            public int CustomerId { get; set; }
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
                //make db call
                Entities.Customer customer;

                customer = await _mediator.Send(new GetCustomerByCustomerIdQuery.Command { CustomerId = request.CustomerId });
                if(customer == null)
                {
                    throw new NotFoundException("Customer with Id [" + request.CustomerId +  "] was not found.");
                }

                return customer.ToCustomerDto();
            }
        }
    }
}
