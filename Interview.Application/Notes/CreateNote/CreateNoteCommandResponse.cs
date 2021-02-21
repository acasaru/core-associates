using Interview.Application.Core.Commands;
using Interview.Application.Core.Exceptions;
using System.Net;


namespace Interview.Application.Notes.CreateNote
{
    public class CreateNoteCommandResponse : BaseHandlerResponseWithContent<NoteDto>
    {
        public CreateNoteCommandResponse(HttpStatusCode status, NoteDto content) : base(status, content) { }

        public CreateNoteCommandResponse(HttpStatusCode status, ApplicationError error) : base(status, error) { }

    }
}
