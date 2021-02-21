using AutoMapper;
using Interview.Application.Core.Exceptions;
using Interview.Application.Interfaces;
using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Interview.Application.Notes.GetNote
{
    public class GetNoteQueryHandler : IRequestHandler<GetNoteQuery, GetNoteQueryResponse>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;

        public GetNoteQueryHandler(
            IApplicationUnitOfWork applicationUnitOfWork,
            IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task<GetNoteQueryResponse> Handle(GetNoteQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var note = await _applicationUnitOfWork.Notes
                .SingleOrDefaultAsync(item => item.NoteId == request.NoteId, cancellationToken);

            if (note == null)
            {
                var applicationError = new ApplicationError(
                    ApplicationConstants.ErrorCodes.BusinessValidationError,
                    string.Format(ApplicationConstants.ErrorMessages.NoteWithIdDoesNotExist, request.NoteId));

                return new GetNoteQueryResponse(HttpStatusCode.NotFound, applicationError);
            }

            var noteDto = _mapper.Map<NoteDto>(note);

            return new GetNoteQueryResponse(HttpStatusCode.OK, noteDto);
        }
    }
}
