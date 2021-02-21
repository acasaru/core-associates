using Interview.Application.Core.Commands;
using Interview.Application.Core.Exceptions;
using System.Collections.Generic;
using System.Net;

namespace Interview.Application.Notes.GetNotesForInvoice
{
    public class GetNotesForInvoiceQueryResponse : BaseHandlerResponseWithContent<List<NoteDto>>
    {
        public GetNotesForInvoiceQueryResponse(HttpStatusCode status, List<NoteDto> content) : base(status, content) { }

        public GetNotesForInvoiceQueryResponse(HttpStatusCode status, ApplicationError error) : base(status, error) { }
    }
}
