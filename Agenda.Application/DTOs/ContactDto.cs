namespace Agenda.Application.DTOs;

public class ContactDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Address { get; set; }
    public DateTime? BirthDate { get; set; }
    public int UserId { get; set; }
}