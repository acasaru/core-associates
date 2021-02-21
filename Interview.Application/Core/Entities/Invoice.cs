using System.Collections.Generic;

namespace Interview.Application.Core.Entitities
{
    public class Invoice: AuditableEntity
    {
        public int InvoiceId { get; set; }
        public string Identifier { get; set; }
        public decimal Amount { get; set; }
        public ICollection<Note> Notes { get; set; }
    }
}
