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
        [Route("Customers/{customerId}/Vehicles")]
        public async Task<ActionResult<VehicleDto>> GetVehicleListByCustomer([FromRoute] int customerId) =>
            Accepted(await Mediator.Send(new GetVehiclesByCustomer.Command { CustomerId = customerId }));

        [HttpPost]
        [ProducesResponseType(202)]
        [Route("Vehicles")]
        public async Task<ActionResult<VehicleDto>> CreateVehicle([FromBody] VehicleDto request) =>
            Accepted(await Mediator.Send(new CreateVehicle.Command { CreateRequest = request }));

        [HttpPut]
        [ProducesResponseType(202)]
        [Route("Vehicles")]
        public async Task<ActionResult<VehicleDto>> UpdateVehicle([FromBody] VehicleDto request) =>
            Accepted(await Mediator.Send(new UpdateVehicle.Command { UpdateRequest = request }));


    }
}
