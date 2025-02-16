using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Autoshop.Application.Common.Models;
using Autoshop.Application.Invoice;

namespace Autoshop.Api.Controllers
{
    [ApiController]    
    public class InvoiceController : ApiController
    {
        [HttpGet]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        [Route("api/Visits/{visitId}/Invoices")]
        public async Task<ActionResult<VisitDto>> GetInvoiceByVisitId([FromRoute] int visitId) =>
            Accepted(await Mediator.Send(new GetInvoiceByVisit.Command { VisitId = visitId }));

        [HttpPost]
        [ProducesResponseType(202)]
        [Route("api/Invoices")]
        public async Task<ActionResult<VisitDto>> CreateInvoice([FromBody] InvoiceDto request) =>
            Accepted(await Mediator.Send(new CreateInvoice.Command { CreateRequest = request }));

         [HttpPut]
         [ProducesResponseType(202)]
         [Route("api/Invoices")]
         public async Task<ActionResult<VisitDto>> UpdateInvoice([FromBody] InvoiceDto request) =>
             Accepted(await Mediator.Send(new UpdateInvoice.Command { UpdateRequest = request }));


    }
}
