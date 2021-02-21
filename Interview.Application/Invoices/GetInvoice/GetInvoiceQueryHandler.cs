using AutoMapper;
using Interview.Application.Core.Entitities;
using Interview.Application.Core.Exceptions;
using Interview.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Interview.Application.Invoices.GetInvoice
{
    public class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, GetInvoiceQueryResponse>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;

        public GetInvoiceQueryHandler(
            IApplicationUnitOfWork applicationUnitOfWork,
            IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task<GetInvoiceQueryResponse> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var invoice = await _applicationUnitOfWork.Invoices
                .SingleOrDefaultAsync(item => item.InvoiceId == request.InvoiceId, cancellationToken);

            if(invoice == null)
            {
                var applicationError = new ApplicationError(
                    ApplicationConstants.ErrorCodes.BusinessValidationError,
                    string.Format(ApplicationConstants.ErrorMessages.InvoiceWithIdDoesNotExist, request.InvoiceId));

                return new GetInvoiceQueryResponse(HttpStatusCode.NotFound, applicationError);
            }

            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);

            return new GetInvoiceQueryResponse(HttpStatusCode.OK, invoiceDto);
        }
    }
}
