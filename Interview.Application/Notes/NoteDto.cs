namespace Interview.Application.Notes
{
    public class NoteDto
    {
        public int NoteId { get; set; }
        public int InvoiceId { get; set; }
        public string Text { get; set; }
        public int CreatedBy { get; set; }
    }
}
