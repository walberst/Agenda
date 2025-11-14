using Agenda.Domain.Entities;
using Agenda.Domain.Interfaces;
using Agenda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Infrastructure.Repositories;


public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _context;

    public ContactRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Contact>> GetAllByUserIdAsync(int userId)
    {
        return await _context.Contacts
                             .Where(c => c.UserId == userId)
                             .ToListAsync();
    }

    public async Task<Contact?> GetByIdAsync(int id)
    {
        return await _context.Contacts.FindAsync(id);
    }

    public async Task AddAsync(Contact contact)
    {
        await _context.Contacts.AddAsync(contact);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Contact contact)
    {
        _context.Contacts.Update(contact);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Contact contact)
    {
        contact.Delete(); 
        _context.Contacts.Update(contact);
        await _context.SaveChangesAsync();
    }

    public async Task RestoreAsync(Contact contact)
    {
        contact.Restore();
        _context.Contacts.Update(contact);
        await _context.SaveChangesAsync();
    }

    public async Task ForceDeleteAsync(Contact contact)
    {
        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();
    }
}
