using Interview.Application.Core.Commands;
using Interview.Application.Core.Exceptions;
using System.Net;

namespace Interview.Application.Notes.GetNote
{
    public class GetNoteQueryResponse: BaseHandlerResponseWithContent<NoteDto>
    {
        public GetNoteQueryResponse(HttpStatusCode status, NoteDto content) : base(status, content) { }

        public GetNoteQueryResponse(HttpStatusCode status, ApplicationError error) : base(status, error) { }

    }
}
