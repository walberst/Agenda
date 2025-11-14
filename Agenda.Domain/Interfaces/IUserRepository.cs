using Agenda.Domain.Entities;

namespace Agenda.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task RestoreAsync(User user);
    Task ForceDeleteAsync(User user);
}
