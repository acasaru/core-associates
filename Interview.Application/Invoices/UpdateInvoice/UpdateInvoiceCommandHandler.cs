using Interview.Application.Core.Exceptions;
using Interview.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Interview.Application.Invoices.UpdateInvoice
{
    public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, UpdateInvoiceCommandResponse>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly ILogger<UpdateInvoiceCommandHandler> _logger;
        private readonly IAutheticatedUserService _userService;

        public UpdateInvoiceCommandHandler(
            IApplicationUnitOfWork applicationUnitOfWork,
            ILogger<UpdateInvoiceCommandHandler> logger,
            IAutheticatedUserService userService
            )
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _logger = logger;
            _userService = userService;
        }
        public async Task<UpdateInvoiceCommandResponse> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var updateContent = request.Content;

            var targetInvoice = await _applicationUnitOfWork.Invoices
                    .SingleOrDefaultAsync(invoice => invoice.InvoiceId == updateContent.InvoiceId);

            if (targetInvoice == null)
            {
                return new UpdateInvoiceCommandResponse(
                   HttpStatusCode.NotFound,
                   new ApplicationError(ApplicationConstants.ErrorCodes.BusinessValidationError,
                       string.Format(ApplicationConstants.ErrorMessages.InvoiceWithIdDoesNotExist, updateContent.InvoiceId)));
            }

            if(targetInvoice.CreatedBy != _userService.GetUserId())
            {
                return new UpdateInvoiceCommandResponse(
                   HttpStatusCode.Forbidden,
                   new ApplicationError(ApplicationConstants.ErrorCodes.AuthorizationError,
                       string.Format(ApplicationConstants.ErrorMessages.InvoiceCreatedByDifferentUser, updateContent.InvoiceId)));
            }

            var otherInvoiceWithSameIdentifier = await _applicationUnitOfWork.Invoices
                .SingleOrDefaultAsync(invoice =>
                    invoice.Identifier == updateContent.Identifier &&
                    invoice.InvoiceId != updateContent.InvoiceId);

            if (otherInvoiceWithSameIdentifier != null)
            {
                return new UpdateInvoiceCommandResponse(
                    HttpStatusCode.BadRequest,
                    new ApplicationError(ApplicationConstants.ErrorCodes.BusinessValidationError,
                        string.Format(ApplicationConstants.ErrorMessages.DuplicateInvoiceIdentifier, updateContent.Identifier)));
            }

            try
            {
                if(updateContent.Identifier != null)
                {
                    targetInvoice.Identifier = updateContent.Identifier;
                }
                
                if(updateContent.Amount.HasValue)
                {
                    targetInvoice.Amount = updateContent.Amount.Value;
                }
                
                _applicationUnitOfWork.Invoices.Update(targetInvoice);

                await _applicationUnitOfWork.CommitAsync(cancellationToken);

                return new UpdateInvoiceCommandResponse(HttpStatusCode.OK);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.Message);

                return new UpdateInvoiceCommandResponse(
                    HttpStatusCode.InternalServerError,
                    new ApplicationError(ApplicationConstants.ErrorCodes.UpdateInvoiceError, exc.Message));
            }
        }
    }
}
