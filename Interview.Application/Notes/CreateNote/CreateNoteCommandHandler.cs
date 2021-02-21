using AutoMapper;
using Interview.Application.Core.Entitities;
using Interview.Application.Core.Exceptions;
using Interview.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Interview.Application.Notes.CreateNote
{
    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, CreateNoteCommandResponse>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly ILogger<CreateNoteCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateNoteCommandHandler(
            IApplicationUnitOfWork applicationUnitOfWork,
            ILogger<CreateNoteCommandHandler> logger,
            IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _logger = logger;
            _mapper = mapper;

        }
        public async Task<CreateNoteCommandResponse> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var currentInvoice = await _applicationUnitOfWork.Invoices
                .SingleOrDefaultAsync(invoice => invoice.InvoiceId == request.Content.InvoiceId);

            if (currentInvoice == null)
            {
                return new CreateNoteCommandResponse(
                    HttpStatusCode.NotFound,
                    new ApplicationError(ApplicationConstants.ErrorCodes.BusinessValidationError,
                        string.Format(ApplicationConstants.ErrorMessages.InvoiceNotFound, request.Content.InvoiceId)));
            }

            var noteEntity = _mapper.Map<Note>(request.Content);

            try
            {
                var createdNote = await _applicationUnitOfWork.Notes.AddAsync(noteEntity);

                await _applicationUnitOfWork.CommitAsync(cancellationToken);

                var createNoteDto = _mapper.Map<NoteDto>(createdNote);

                return new CreateNoteCommandResponse(HttpStatusCode.Created, createNoteDto);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.Message);

                return new CreateNoteCommandResponse(
                    HttpStatusCode.InternalServerError,
                    new ApplicationError(ApplicationConstants.ErrorCodes.CreateNoteError, exc.Message));
            }
        }
    }
}
