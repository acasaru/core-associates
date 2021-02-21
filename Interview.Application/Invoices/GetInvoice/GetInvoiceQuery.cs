using MediatR;

namespace Interview.Application.Invoices.GetInvoice
{
    public class GetInvoiceQuery: IRequest<GetInvoiceQueryResponse>
    {
        public int InvoiceId { get; }
        
        public GetInvoiceQuery(int invoiceId)
        {
            InvoiceId = invoiceId;
        }
    }
}
