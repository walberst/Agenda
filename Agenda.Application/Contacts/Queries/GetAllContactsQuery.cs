using Agenda.Application.DTOs;
using Agenda.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Agenda.Application.Contacts.Queries;

public record GetAllContactsQuery(int UserId) : IRequest<IEnumerable<ContactDto>>;

public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, IEnumerable<ContactDto>>
{
    private readonly IContactRepository _contactRepository;
    private readonly IMapper _mapper;

    public GetAllContactsQueryHandler(IContactRepository contactRepository, IMapper mapper)
    {
        _contactRepository = contactRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ContactDto>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
    {
        var contacts = await _contactRepository.GetAllByUserIdAsync(request.UserId);
        return _mapper.Map<IEnumerable<ContactDto>>(contacts);
    }
}
