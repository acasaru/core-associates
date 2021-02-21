using FluentValidation;

namespace Interview.Application.Notes.UpdateNote
{
    public class UpdateNoteDtoValidator : AbstractValidator<UpdateNoteDto>
    {
        public UpdateNoteDtoValidator()
        {
            RuleFor(x => x.NoteId)
               .NotNull()
               .WithMessage(ApplicationConstants.ErrorMessages.NoteIdFieldIsMandatory);

            RuleFor(x => x.NoteId.Value)
                .Must(x => x > 0)
                .When(x => x.NoteId.HasValue)
                .WithMessage(ApplicationConstants.ErrorMessages.NoteIdFieldValueNotZero);

            RuleFor(x => x.Text)
               .NotNull()
               .WithMessage(ApplicationConstants.ErrorMessages.TextFieldIsMandatory)
               .MaximumLength(2048)
               .WithMessage(ApplicationConstants.ErrorMessages.TextFieldHasMaxLength);

        }
    }
}
