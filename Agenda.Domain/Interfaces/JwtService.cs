using Agenda.Domain.Entities;

namespace Agenda.Domain.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}
