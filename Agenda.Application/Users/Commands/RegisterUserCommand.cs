using Agenda.Application.DTOs;
using Agenda.Domain.Entities;
using Agenda.Domain.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Agenda.Application.Users.Commands;

public record RegisterUserCommand(UserDto User, string Password) : IRequest<UserDto>;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(IValidator<UserDto> userValidator)
    {
        RuleFor(x => x.User)
            .SetValidator(userValidator); 

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres.");
    }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = _mapper.Map<User>(request.User);
        user.PasswordHash = passwordHash;

        await _userRepository.AddAsync(user);

        return _mapper.Map<UserDto>(user);
    }
}
