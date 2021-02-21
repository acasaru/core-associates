using MediatR;


namespace Interview.Application.Notes.CreateNote
{
    public class CreateNoteCommand : IRequest<CreateNoteCommandResponse>
    {
        public CreateNoteDto Content { get; }

        public CreateNoteCommand(CreateNoteDto createInvoiceContent)
        {
            Content = createInvoiceContent;
        }
    }
}
