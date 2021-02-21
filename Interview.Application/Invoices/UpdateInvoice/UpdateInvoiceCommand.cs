using MediatR;

namespace Interview.Application.Invoices.UpdateInvoice
{
    public class UpdateInvoiceCommand : IRequest<UpdateInvoiceCommandResponse>
    {
        public UpdateInvoiceDto Content { get; }

        public UpdateInvoiceCommand(UpdateInvoiceDto updateInvoiceContent)
        {
            Content = updateInvoiceContent;
        }
    }
}
