using Agenda.Domain.Common;

namespace Agenda.Domain.Entities;
public class Contact : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty; 
    public string Phone { get; set; } = string.Empty; 
    public string? Address { get; set; } 
    public DateTime? BirthDate { get; set; }

    
    public int UserId { get; set; }
    public User? User { get; set; }
}