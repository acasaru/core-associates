using MediatR;

namespace Interview.Application.Invoices.CreateInvoice
{
    public class CreateInvoiceCommand: IRequest<CreateInvoiceCommandResponse>
    {
        public CreateInvoiceDto Content { get; }

        public CreateInvoiceCommand(CreateInvoiceDto createInvoiceContent)
        {
            Content = createInvoiceContent;
        }
    }
}
