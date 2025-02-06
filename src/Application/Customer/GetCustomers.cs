using Autoshop.Application.Queries;
using MediatR;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace Autoshop.Application.Customer
{
    public static class GetCustomers
    {
        public class Command : IRequest<List<CustomerDto>>
        {
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, List<CustomerDto>>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(IMediator mediator, ILogger<CommandHandler> logger)
            {
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<List<CustomerDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                List<CustomerDto> result = new List<CustomerDto>();

                //make db call
                List<Entities.Customer> customerList;

                customerList = await _mediator.Send(new GetCustomersQuery.Command { 
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber });

                foreach(var customer in customerList)
                {
                    result.Add(customer.ToCustomerDto());
                }

                return result;
            }
        }
    }
}
