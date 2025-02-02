using Autoshop.Application.Queries;
using MediatR;
using Autoshop.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace Autoshop.Application.Customer
{
    public static class GetCustomers
    {
        public class Command : IRequest<List<GetCustomersResponse>>
        {
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, List<GetCustomersResponse>>
        {
            private readonly IMediator _mediator;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(IMediator mediator, ILogger<CommandHandler> logger)
            {
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<List<GetCustomersResponse>> Handle(Command request, CancellationToken cancellationToken)
            {
                List<GetCustomersResponse> result = new List<GetCustomersResponse>();

                //make db call
                List<Entities.Customer> customerList;

                customerList = await _mediator.Send(new GetCustomersQuery.Command { 
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber });

                //TODO: Use Mapper here

                foreach(var customer in customerList)
                {
                    result.Add(new GetCustomersResponse {
                        CustomerId = customer.CustomerId,
                        Name = customer.Name,
                        Email = customer.Email,
                        Address = customer.Address,
                        PhoneNumber = customer.PhoneNumber
                    });
                }

                return result;
            }
        }
    }
}
