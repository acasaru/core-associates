using MediatR;

namespace Interview.Application.Notes.GetNotesForInvoice
{
    public class GetNotesForInvoiceQuery : IRequest<GetNotesForInvoiceQueryResponse>
    {
        public int InvoiceId { get; }

        public GetNotesForInvoiceQuery(int invoiceId)
        {
            InvoiceId = invoiceId;
        }
    }
}
