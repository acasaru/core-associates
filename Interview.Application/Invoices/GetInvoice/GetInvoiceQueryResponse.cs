using Interview.Application.Core.Commands;
using Interview.Application.Core.Exceptions;
using System.Net;

namespace Interview.Application.Invoices.GetInvoice
{
    public class GetInvoiceQueryResponse: BaseHandlerResponseWithContent<InvoiceDto>
    {
        public GetInvoiceQueryResponse(HttpStatusCode status, InvoiceDto content) : base(status, content) { }

        public GetInvoiceQueryResponse(HttpStatusCode status, ApplicationError error) : base(status, error) { }
        
    }
}
