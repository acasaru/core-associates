using FluentValidation;
namespace Interview.Application.Invoices.UpdateInvoice
{
    public class UpdateInvoiceDtoValidator : AbstractValidator<UpdateInvoiceDto>
    {
        public UpdateInvoiceDtoValidator()
        {
            RuleFor(x => x.Identifier)
                .MaximumLength(64)
                .When(x => x != null)
                .WithMessage(ApplicationConstants.ErrorMessages.IdentifierFieldHasMaxLength);

            RuleFor(x => x.Amount.Value)
                .Must(x => x != decimal.Zero)
                .When(x => x.Amount.HasValue)
                .WithMessage(ApplicationConstants.ErrorMessages.AmountFieldValueNotZero);

            RuleFor(x => x.InvoiceId)
               .NotNull()
               .WithMessage(ApplicationConstants.ErrorMessages.InvoiceIdFieldIsMandatory);

            RuleFor(x => x.InvoiceId.Value)
                .Must(x => x > 0)
                .When(x => x.InvoiceId.HasValue)
                .WithMessage(ApplicationConstants.ErrorMessages.InvoiceIdFieldValueNotZero);

            RuleFor(x => new { x.Identifier, x.Amount })
                .Must(x => x.Amount.HasValue || !string.IsNullOrEmpty(x.Identifier))
                .WithMessage(ApplicationConstants.ErrorMessages.UpdateInvoiceAtLeastOneFieldRequired);



        }
    }
}
