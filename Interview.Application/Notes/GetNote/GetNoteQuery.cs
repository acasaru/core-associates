using MediatR;

namespace Interview.Application.Notes.GetNote
{
    public class GetNoteQuery: IRequest<GetNoteQueryResponse>
    {
        public int NoteId { get; }

        public GetNoteQuery(int noteId)
        {
            NoteId = noteId;
        }
    }
}
