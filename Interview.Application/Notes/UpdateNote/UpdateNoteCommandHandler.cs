using Interview.Application.Core.Exceptions;
using Interview.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Interview.Application.Notes.UpdateNote
{
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, UpdateNoteCommandResponse>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly ILogger<UpdateNoteCommandHandler> _logger;
        private readonly IAutheticatedUserService _userService;

        public UpdateNoteCommandHandler(
            IApplicationUnitOfWork applicationUnitOfWork,
            ILogger<UpdateNoteCommandHandler> logger,
            IAutheticatedUserService userService
            )
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _logger = logger;
            _userService = userService;
        }
        public async Task<UpdateNoteCommandResponse> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var updateContent = request.Content;

            var targetNote = await _applicationUnitOfWork.Notes
                    .SingleOrDefaultAsync(note => note.NoteId == updateContent.NoteId);

            if (targetNote == null)
            {
                return new UpdateNoteCommandResponse(
                   HttpStatusCode.NotFound,
                   new ApplicationError(ApplicationConstants.ErrorCodes.BusinessValidationError,
                       string.Format(ApplicationConstants.ErrorMessages.NoteWithIdDoesNotExist, updateContent.NoteId)));
            }

            if (targetNote.CreatedBy != _userService.GetUserId())
            {
                return new UpdateNoteCommandResponse(
                   HttpStatusCode.Forbidden,
                   new ApplicationError(ApplicationConstants.ErrorCodes.AuthorizationError,
                       string.Format(ApplicationConstants.ErrorMessages.NoteCreatedByDifferentUser, updateContent.NoteId)));
            }

            try
            {
                targetNote.Text = updateContent.Text;

                _applicationUnitOfWork.Notes.Update(targetNote);

                await _applicationUnitOfWork.CommitAsync(cancellationToken);

                return new UpdateNoteCommandResponse(HttpStatusCode.OK);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.Message);

                return new UpdateNoteCommandResponse(
                    HttpStatusCode.InternalServerError,
                    new ApplicationError(ApplicationConstants.ErrorCodes.UpdateNoteError, exc.Message));
            }
        }
    }
}
