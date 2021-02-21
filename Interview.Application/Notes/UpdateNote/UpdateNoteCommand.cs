using MediatR;

namespace Interview.Application.Notes.UpdateNote
{
    public class UpdateNoteCommand : IRequest<UpdateNoteCommandResponse>
    {
        public UpdateNoteDto Content { get; }

        public UpdateNoteCommand(UpdateNoteDto updateNoteContent)
        {
            Content = updateNoteContent;
        }
    }
}
