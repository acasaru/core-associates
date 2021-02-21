using System;

namespace Interview.Application.Invoices
{
    public class InvoiceDto
    {
        public int InvoiceId { get; set; }
        public string Identifier { get; set; }
        public decimal Amount { get; set; }
        public int CreatedBy { get; set; }
    }
}
