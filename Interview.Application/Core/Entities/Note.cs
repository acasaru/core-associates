namespace Interview.Application.Core.Entitities
{
    public class Note: AuditableEntity
    {
        public int NoteId { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public string Text { get; set; }
    }
}
