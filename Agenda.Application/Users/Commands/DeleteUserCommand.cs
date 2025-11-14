using MediatR;
using Agenda.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Agenda.Application.Users.Commands;

public record DeleteUserCommand(int Id) : IRequest<bool>;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var existing = await _userRepository.GetByIdAsync(request.Id);
        if (existing == null)
            return false;

        await _userRepository.DeleteAsync(existing);
        return true;
    }
}
