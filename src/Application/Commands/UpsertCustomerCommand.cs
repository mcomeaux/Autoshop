using MediatR;
using Microsoft.EntityFrameworkCore;
using Autoshop.Application.Interfaces;
using Autoshop.Application.Entities;

namespace Autoshop.Application.Commands
{
    public class UpsertCustomerCommand
    {
        public class Command : IRequest<Entities.Customer>
        {
            public int CustomerId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Entities.Customer>
        {
            private readonly IApplicationDbContext _dbContext;

            public CommandHandler(IApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }
            public async Task<Entities.Customer> Handle(Command request, CancellationToken cancellationToken)
            {
                var customer = await _dbContext.Customers.AddAsync(new Entities.Customer
                {
                    CustomerId = request.CustomerId,
                    Name = request.Name,
                    Email = request.Email,
                    Address = request.Address,
                    PhoneNumber = request.PhoneNumber
                });

                try
                {
                    var result = await _dbContext.SaveChangesAsync(cancellationToken);

                    return customer.Entity;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}