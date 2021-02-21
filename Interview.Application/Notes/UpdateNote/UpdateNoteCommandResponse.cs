using Interview.Application.Core.Commands;
using Interview.Application.Core.Exceptions;
using System.Net;

namespace Interview.Application.Notes.UpdateNote
{
    public class UpdateNoteCommandResponse : BaseHandlerResponse
    {
        public UpdateNoteCommandResponse(HttpStatusCode status) : base(status) { }

        public UpdateNoteCommandResponse(HttpStatusCode status, ApplicationError error) : base(status, error) { }

    }
}
