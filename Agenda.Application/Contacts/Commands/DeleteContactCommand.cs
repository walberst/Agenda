using MediatR;
using Agenda.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Agenda.Application.Contacts.Commands;

public record DeleteContactCommand(int Id) : IRequest<bool>;

public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, bool>
{
    private readonly IContactRepository _repository;

    public DeleteContactCommandHandler(IContactRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        var contact = await _repository.GetByIdAsync(request.Id);
        if (contact == null)
            return false;

        await _repository.DeleteAsync(contact);
        return true;
    }
}
