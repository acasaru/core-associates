using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Interview.Application;
using Interview.Application.Invoices;
using Interview.Application.Invoices.CreateInvoice;
using Interview.Application.Invoices.GetInvoice;
using Interview.Application.Invoices.UpdateInvoice;
using Interview.Application.Notes;
using Interview.Application.Notes.GetNotesForInvoice;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interview.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ApiBaseController
    {
        public InvoicesController(IMediator mediator): base(mediator){}

        [HttpGet]
        [Route("{invoiceId}")]
        [Authorize(
            AuthenticationSchemes = ApplicationConstants.Authentication.AuthenticationScheme,
            Roles = ApplicationConstants.AuthorizationRoles.AllRoles)]
        public async Task<ActionResult<InvoiceDto>> GetInvoice([Required] [FromRoute] int invoiceId, CancellationToken cancellationToken)
        {
           var response =  await _mediator.Send(new GetInvoiceQuery(invoiceId), cancellationToken);

            return ProcessResult(response);
        }

        [HttpGet]
        [Route("{invoiceId}/notes")]
        [Authorize(
            AuthenticationSchemes = ApplicationConstants.Authentication.AuthenticationScheme,
            Roles = ApplicationConstants.AuthorizationRoles.AllRoles)]
        public async Task<ActionResult<List<NoteDto>>> GetNotesForInvoice([Required] [FromRoute] int invoiceId, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetNotesForInvoiceQuery(invoiceId), cancellationToken);

            return ProcessResult(response);
        }

        [HttpPost]
        [Authorize(
            AuthenticationSchemes = ApplicationConstants.Authentication.AuthenticationScheme, 
            Roles = ApplicationConstants.AuthorizationRoles.Admin)]
        public async Task<ActionResult<InvoiceDto>> CreateInvoice([FromBody] CreateInvoiceDto createInvoiceModel, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new CreateInvoiceCommand(createInvoiceModel), cancellationToken);

            return ProcessResult(response);
        }

        [HttpPut]
        [Authorize(
            AuthenticationSchemes = ApplicationConstants.Authentication.AuthenticationScheme,
            Roles = ApplicationConstants.AuthorizationRoles.Admin)]
        public async Task<ActionResult> UpdateInvoice([FromBody] UpdateInvoiceDto updateInvoiceModel, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new UpdateInvoiceCommand(updateInvoiceModel), cancellationToken);

            return ProcessResult(response);
        }

    }
}