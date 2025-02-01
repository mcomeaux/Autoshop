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
            public int? CustomerId { get; set; }
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
                Entities.Customer customer;
                if(request.CustomerId != null)
                {
                    customer = await _dbContext.Customers
                    .FirstOrDefaultAsync(c => c.CustomerId == request.CustomerId);
                    if(customer == null){
                        throw(new Exception());//TODO: not found exception
                    }
                    customer.Name = request.Name;
                    customer.Email = request.Email;
                    customer.PhoneNumber = request.PhoneNumber;
                    customer.Address = request.Address;
                }
                else{
                    var createdCustomer = await _dbContext.Customers.AddAsync(new Entities.Customer
                    {
                        Name = request.Name,
                        Email = request.Email,
                        Address = request.Address,
                        PhoneNumber = request.PhoneNumber
                    });
                    customer = createdCustomer.Entity;
                }
                

                try
                {
                    var result = await _dbContext.SaveChangesAsync(cancellationToken);

                    return customer;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}