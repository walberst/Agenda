using FluentValidation;
using Agenda.Application.DTOs;

namespace Agenda.Application.Contacts.Validators;

public class ContactValidator : AbstractValidator<ContactDto>
{
    public ContactValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(255).WithMessage("Nome deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Formato de email inválido.")
            .MaximumLength(255).WithMessage("Email deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Telefone é obrigatório.")
            .Matches(@"^\(?\d{2}\)?\s?\d{4,5}-?\d{4}$")
            .WithMessage("Telefone deve estar no formato (XX) XXXXX-XXXX.");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("UserId inválido.");
    }
}
