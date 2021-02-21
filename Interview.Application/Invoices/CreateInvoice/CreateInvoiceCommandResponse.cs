using Interview.Application.Core;
using Interview.Application.Core.Commands;
using Interview.Application.Core.Exceptions;
using System.Net;

namespace Interview.Application.Invoices.CreateInvoice
{
    public class CreateInvoiceCommandResponse : BaseHandlerResponseWithContent<InvoiceDto>
    {
        public CreateInvoiceCommandResponse(HttpStatusCode status, InvoiceDto content) : base(status, content) { }

        public CreateInvoiceCommandResponse(HttpStatusCode status, ApplicationError error) : base(status, error) { }

    }
}
