using Autoshop.Application.Common.Models;

namespace Autoshop.Application.Customer
{
    public static class CustomerToDtoMapper
    {
        public static CustomerDto ToCustomerDto(this Entities.Customer customer)
        {
            return new CustomerDto{
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            };
        }
    }
}