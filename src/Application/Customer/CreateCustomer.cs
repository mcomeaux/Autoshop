using Autoshop.Application.Queries;
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

                //make db call
                // Entities.Customer customer;

                // customer = await _mediator.Send(new GetCustomersQuery.Command { Name = request.Name });

                // //TODO: Use Mapper here

                // foreach(var customer in customerList)
                // {
                //     result.Add(new GetCustomersResponse {
                //         CustomerId = customer.CustomerId,
                //         Name = customer.Name,
                //         Email = customer.Email,
                //         Address = customer.Address,
                //         PhoneNumber = customer.PhoneNumber
                //     });
                // }

                return result;
            }
        }
    }
}