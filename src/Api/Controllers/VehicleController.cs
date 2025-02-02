using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Autoshop.Application.Common.Models;
using Autoshop.Application.Vehicle;

namespace Autoshop.Api.Controllers
{
    [ApiController]
    public class VehicleController : ApiController
    {
        
        /// <summary>
        /// Get a List of Vehicles by CustomerId
        /// </summary>
        /// <param name="customerId"></param>
        [HttpGet]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        [Route("api/Customers/{customerId}/Vehicles")]
        public async Task<ActionResult<GetCustomersResponse>> GetVehicleListByCustomer([FromRoute] int customerId) =>
            Accepted(await Mediator.Send(new GetVehiclesByCustomer.Command { CustomerId = customerId }));

    }
}
