using FluentValidation;

namespace Interview.Application.Invoices.CreateInvoice
{
    public class CreateInvoiceDtoValidator
    {
        public class CreateInvoiceModelValidator : AbstractValidator<CreateInvoiceDto>
        {
            public CreateInvoiceModelValidator()
            {
                RuleFor(x => x.Identifier)
                    .NotEmpty()
                    .WithMessage(ApplicationConstants.ErrorMessages.IdentifierFieldIsMandatory)
                    .MaximumLength(64)
                    .WithMessage(ApplicationConstants.ErrorMessages.IdentifierFieldHasMaxLength);

                RuleFor(x => x.Amount)
                    .NotNull()
                    .WithMessage(ApplicationConstants.ErrorMessages.AmountFieldIsMandatory);

                RuleFor(x => x.Amount.Value)
                    .Must(x => x != decimal.Zero)
                    .When(x => x.Amount.HasValue)
                    .WithMessage(ApplicationConstants.ErrorMessages.AmountFieldValueNotZero);

            }
        }
    }
}
