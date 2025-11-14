using MediatR;
using Agenda.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Agenda.Domain.Entities;

namespace Agenda.Application.Users.Commands;

public record AuthenticateUserCommand(string Email, string Password) : IRequest<string>;

public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthenticateUserCommandHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<string> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
            return string.Empty;

        var hashedPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!hashedPassword)
            return string.Empty;

        return _jwtService.GenerateToken(user);
    }
}
