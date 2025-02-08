using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Autoshop.Application.Common.Models;
using Autoshop.Application.Visit;

namespace Autoshop.Api.Controllers
{
    [ApiController]
    public class VisitController : ApiController
    {
        
        /// <summary>
        /// Get a List of Active Visits
        /// </summary>
        [HttpGet]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        [Route("api/Visits/Active")]
        public async Task<ActionResult<VisitDto>> GetActiveVisitList() =>
            Accepted(await Mediator.Send(new GetActiveVisits.Command {  }));

        [HttpPost]
        [ProducesResponseType(202)]
        public async Task<ActionResult<VisitDto>> CreateVisit([FromBody] VisitDto request) =>
            Accepted(await Mediator.Send(new CreateVisit.Command { CreateRequest = request }));

         [HttpPut]
         [ProducesResponseType(202)]
         public async Task<ActionResult<VisitDto>> UpdateVisit([FromBody] VisitDto request) =>
             Accepted(await Mediator.Send(new UpdateVisit.Command { UpdateRequest = request }));


    }
}
