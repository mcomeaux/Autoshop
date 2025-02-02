using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Autoshop.Application.Common.Models;
using Autoshop.Application.Customer;

namespace Autoshop.Api.Controllers
{
    [Route("api/Customers")]
    [ApiController]
    public class CustomerController : ApiController
    {
        
        /// <summary>
        /// Get a List of Customers by Name
        /// </summary>
        /// <param name="name"></param>
        [HttpGet]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<GetCustomersResponse>> GetCustomers([FromQuery] string name = "") =>
            Accepted(await Mediator.Send(new GetCustomers.Command { Name = name }));


        [HttpGet]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Route("{customerId}")]
        public async Task<ActionResult<GetCustomersResponse>> GetCustomerById([FromRoute] int customerId) =>
            Accepted(await Mediator.Send(new GetCustomer.Command { CustomerId = customerId }));

        [HttpPost]
        [ProducesResponseType(202)]
        public async Task<ActionResult<GetCustomersResponse>> CreateCustomer([FromBody] CustomerRequest request) =>
            Accepted(await Mediator.Send(new CreateCustomer.Command { CreateRequest = request }));

        [HttpPut]
        [ProducesResponseType(202)]
        public async Task<ActionResult<GetCustomersResponse>> UpdateCustomer([FromBody] CustomerRequest request) =>
            Accepted(await Mediator.Send(new UpdateCustomer.Command { UpdateRequest = request }));

    }
}
