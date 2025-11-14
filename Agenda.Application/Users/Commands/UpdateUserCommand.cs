using MediatR;
using Agenda.Application.DTOs;
using Agenda.Domain.Interfaces;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;

namespace Agenda.Application.Users.Commands;

public record UpdateUserCommand(int Id, UserDto User) : IRequest<UserDto?>;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existing = await _userRepository.GetByIdAsync(request.Id);
        if (existing == null)
            return null;

        _mapper.Map(request.User, existing);
        await _userRepository.UpdateAsync(existing);

        return _mapper.Map<UserDto>(existing);
    }
}
