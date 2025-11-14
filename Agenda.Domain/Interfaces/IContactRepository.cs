using Agenda.Domain.Entities;  

namespace Agenda.Domain.Interfaces;

public interface IContactRepository
{
    Task<IEnumerable<Contact>> GetAllByUserIdAsync(int userId);
    Task<Contact?> GetByIdAsync(int id);
    Task AddAsync(Contact contact);
    Task UpdateAsync(Contact contact);
    Task DeleteAsync(Contact contact);
    Task RestoreAsync(Contact contact);
    Task ForceDeleteAsync(Contact contact);
}

