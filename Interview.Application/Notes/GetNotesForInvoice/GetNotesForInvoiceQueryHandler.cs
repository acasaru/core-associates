using AutoMapper;
using Interview.Application.Core.Exceptions;
using Interview.Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Interview.Application.Notes.GetNotesForInvoice
{
    public class GetNotesForInvoiceQueryHandler : IRequestHandler<GetNotesForInvoiceQuery, GetNotesForInvoiceQueryResponse>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;

        public GetNotesForInvoiceQueryHandler(
            IApplicationUnitOfWork applicationUnitOfWork,
            IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task<GetNotesForInvoiceQueryResponse> Handle(GetNotesForInvoiceQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var invoice = await _applicationUnitOfWork.Invoices
                .SingleOrDefaultAsync(item => item.InvoiceId == request.InvoiceId, item=> item.Notes, cancellationToken);

            if (invoice == null)
            {
                var applicationError = new ApplicationError(
                   ApplicationConstants.ErrorCodes.BusinessValidationError,
                   string.Format(ApplicationConstants.ErrorMessages.InvoiceWithIdDoesNotExist, request.InvoiceId));

                return new GetNotesForInvoiceQueryResponse(HttpStatusCode.NotFound, applicationError);
            }

            var notesDto = _mapper.Map<List<NoteDto>>(invoice.Notes)?? new List<NoteDto>();

            return new GetNotesForInvoiceQueryResponse(HttpStatusCode.OK, notesDto);
        }
    }
}
