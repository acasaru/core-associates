using Interview.Application.Core.Commands;
using Interview.Application.Core.Exceptions;
using System.Net;

namespace Interview.Application.Invoices.UpdateInvoice
{
    public class UpdateInvoiceCommandResponse: BaseHandlerResponse
    {
        public UpdateInvoiceCommandResponse(HttpStatusCode status) : base(status) { }

        public UpdateInvoiceCommandResponse(HttpStatusCode status, ApplicationError error) : base(status, error) { }

    }
}

