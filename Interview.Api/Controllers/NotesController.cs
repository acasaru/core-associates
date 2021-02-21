using Interview.Application;
using Interview.Application.Notes;
using Interview.Application.Notes.CreateNote;
using Interview.Application.Notes.GetNote;
using Interview.Application.Notes.UpdateNote;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Interview.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ApiBaseController
    {
        public NotesController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        [Route("{noteId}")]
        [Authorize(
            AuthenticationSchemes = ApplicationConstants.Authentication.AuthenticationScheme,
            Roles = ApplicationConstants.AuthorizationRoles.AllRoles)]
        public async Task<ActionResult<NoteDto>> GetNote([Required] [FromRoute] int noteId, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetNoteQuery(noteId), cancellationToken);

            return ProcessResult(response);
        }

        [HttpPost]
        [Authorize(
            AuthenticationSchemes = ApplicationConstants.Authentication.AuthenticationScheme,
            Roles = ApplicationConstants.AuthorizationRoles.Admin)]
        public async Task<ActionResult<NoteDto>> CreateNote([FromBody] CreateNoteDto createNoteModel, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new CreateNoteCommand(createNoteModel), cancellationToken);

            return ProcessResult(response);
        }

        [HttpPut]
        [Authorize(
            AuthenticationSchemes = ApplicationConstants.Authentication.AuthenticationScheme,
            Roles = ApplicationConstants.AuthorizationRoles.Admin)]
        public async Task<ActionResult> UpdateInvoice([FromBody] UpdateNoteDto updateNoteModel, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new UpdateNoteCommand(updateNoteModel), cancellationToken);

            return ProcessResult(response);
        }
    }
}
