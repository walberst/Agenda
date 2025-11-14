using Agenda.Application.DTOs;
using Agenda.Domain.Entities;
using Agenda.Domain.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Agenda.Application.Contacts.Commands;

public record CreateContactCommand(ContactDto Contact) : IRequest<ContactDto>;

public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
{
    public CreateContactCommandValidator(IValidator<ContactDto> contactValidator)
    {
        RuleFor(x => x.Contact)
            .SetValidator(contactValidator); 
    }
}

public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, ContactDto>
{
    private readonly IContactRepository _contactRepository;
    private readonly IMapper _mapper;

    public CreateContactCommandHandler(IContactRepository contactRepository, IMapper mapper)
    {
        _contactRepository = contactRepository;
        _mapper = mapper;
    }

    public async Task<ContactDto> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        var contact = _mapper.Map<Contact>(request.Contact);
        await _contactRepository.AddAsync(contact);
        return _mapper.Map<ContactDto>(contact);
    }
}
