using MediatR;
using Agenda.Application.DTOs;
using Agenda.Domain.Interfaces;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;

namespace Agenda.Application.Contacts.Queries;

public record GetContactByIdQuery(int Id) : IRequest<ContactDto?>;

public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, ContactDto?>
{
    private readonly IContactRepository _repository;
    private readonly IMapper _mapper;

    public GetContactByIdQueryHandler(IContactRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ContactDto?> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
    {
        var contact = await _repository.GetByIdAsync(request.Id);
        return contact == null ? null : _mapper.Map<ContactDto>(contact);
    }
}
