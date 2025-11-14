using MediatR;
using Agenda.Application.DTOs;
using Agenda.Domain.Interfaces;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using Agenda.Domain.Entities;

namespace Agenda.Application.Contacts.Commands;

public record UpdateContactCommand(int Id, ContactDto Contact) : IRequest<ContactDto?>;

public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, ContactDto?>
{
    private readonly IContactRepository _repository;
    private readonly IMapper _mapper;

    public UpdateContactCommandHandler(IContactRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ContactDto?> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.Id);
        if (existing == null)
            return null;

        _mapper.Map(request.Contact, existing);
        await _repository.UpdateAsync(existing);
        return _mapper.Map<ContactDto>(existing);
    }
}
