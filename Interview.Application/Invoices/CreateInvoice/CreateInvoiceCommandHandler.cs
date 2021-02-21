using AutoMapper;
using Interview.Application.Core.Entitities;
using Interview.Application.Core.Exceptions;
using Interview.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Interview.Application.Invoices.CreateInvoice
{
    public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, CreateInvoiceCommandResponse>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly ILogger<CreateInvoiceCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateInvoiceCommandHandler(
            IApplicationUnitOfWork applicationUnitOfWork,
            ILogger<CreateInvoiceCommandHandler> logger,
            IMapper mapper
            )
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<CreateInvoiceCommandResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var sameIdentifierInvoice = await _applicationUnitOfWork.Invoices
                .SingleOrDefaultAsync(invoice => invoice.Identifier == request.Content.Identifier);

            if(sameIdentifierInvoice != null)
            {
                return new CreateInvoiceCommandResponse(
                    HttpStatusCode.BadRequest,
                    new ApplicationError(ApplicationConstants.ErrorCodes.BusinessValidationError, 
                        string.Format(ApplicationConstants.ErrorMessages.SameIdentifierInvoice, request.Content.Identifier)));
            }

            var invoiceEntity = _mapper.Map<Invoice>(request.Content);

            try
            {
                var createdInvoice = await _applicationUnitOfWork.Invoices.AddAsync(invoiceEntity);

                await _applicationUnitOfWork.CommitAsync(cancellationToken);

                var createdInvoiceDto = _mapper.Map<InvoiceDto>(createdInvoice);

                return new CreateInvoiceCommandResponse(HttpStatusCode.Created, createdInvoiceDto);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.Message);

                return new CreateInvoiceCommandResponse(
                    HttpStatusCode.InternalServerError,
                    new ApplicationError(ApplicationConstants.ErrorCodes.CreateInvoiceError, exc.Message));
            }
        }
    }
}
