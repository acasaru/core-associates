using FluentValidation;

namespace Interview.Application.Notes.CreateNote
{
    public class CreateNoteDtoValidator : AbstractValidator<CreateNoteDto>
    {
        public CreateNoteDtoValidator()
        {
            RuleFor(x => x.InvoiceId)
                .NotNull()
                .WithMessage(ApplicationConstants.ErrorMessages.InvoiceIdFieldIsMandatory);

            RuleFor(x => x.InvoiceId.Value)
               .Must(x => x > 0)
               .When(x => x.InvoiceId.HasValue)
               .WithMessage(ApplicationConstants.ErrorMessages.InvoiceIdFieldValueNotZero);

            RuleFor(x => x.Text)
               .NotNull()
               .WithMessage(ApplicationConstants.ErrorMessages.TextFieldIsMandatory)
               .MaximumLength(2048)
               .WithMessage(ApplicationConstants.ErrorMessages.TextFieldHasMaxLength);

        }
    }
}
