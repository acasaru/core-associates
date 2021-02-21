namespace Interview.Application.Invoices.UpdateInvoice
{
    public class UpdateInvoiceDto
    {
        public int? InvoiceId { get; set; }
        public string Identifier { get; set; }
        public decimal? Amount { get; set; }
    }
}
