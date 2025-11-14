using FluentValidation;
using Agenda.Application.DTOs;

namespace Agenda.Application.Users.Validators;

public class UserValidator : AbstractValidator<UserDto>
{
    public UserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do usuário é obrigatório.")
            .MaximumLength(255).WithMessage("O nome do usuário deve ter no máximo 255 caracteres.")
            .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("O nome não pode conter apenas espaços em branco.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email do usuário é obrigatório.")
            .MaximumLength(255).WithMessage("O email do usuário deve ter no máximo 255 caracteres.")
            .EmailAddress().WithMessage("Formato de email inválido."); 
    }
}
