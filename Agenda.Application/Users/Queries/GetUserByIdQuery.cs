using MediatR;
using Agenda.Domain.Entities;
using Agenda.Domain.Interfaces;
using Agenda.Application.DTOs;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;

namespace Agenda.Application.Users.Queries;

public record GetUserByIdQuery(int Id) : IRequest<UserDto?>;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }
}
