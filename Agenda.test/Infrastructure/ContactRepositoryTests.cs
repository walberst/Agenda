using Agenda.Domain.Entities;
using Agenda.Infrastructure.Repositories;
using Agenda.test.Utils;
using System.Linq;
using Xunit;

namespace Agenda.test.Infrastructure;

public class ContactRepositoryTests
{
    [Fact]
    public async void Should_Add_Contact()
    {
        var context = TestDbContextFactory.Create();
        var repo = new ContactRepository(context);

        var contact = new Contact { Name = "Test", Email = "test@mail.com", Phone = "(11) 99999-8888", UserId = 1 };
        await repo.AddAsync(contact);

        var saved = context.Contacts.First();
        Assert.Equal("Test", saved.Name);
    }

    [Fact]
    public async void Should_Update_Contact()
    {
        var context = TestDbContextFactory.Create();
        var repo = new ContactRepository(context);

        var contact = new Contact { Name = "Old", Email = "old@mail.com", Phone = "(11) 99999-8888", UserId = 1 };
        await repo.AddAsync(contact);

        contact.Name = "New";
        await repo.UpdateAsync(contact);

        var updated = context.Contacts.First();
        Assert.Equal("New", updated.Name);
    }

    [Fact]
    public async void Should_Delete_And_Restore_Contact()
    {
        var context = TestDbContextFactory.Create();
        var repo = new ContactRepository(context);

        var contact = new Contact { Name = "Temp", Email = "temp@mail.com", Phone = "(11) 99999-8888", UserId = 1 };
        await repo.AddAsync(contact);

        await repo.DeleteAsync(contact);
        Assert.True(contact.IsDeleted);

        await repo.RestoreAsync(contact);
        Assert.False(contact.IsDeleted);
    }

    [Fact]
    public async void Should_Force_Delete_Contact()
    {
        var context = TestDbContextFactory.Create();
        var repo = new ContactRepository(context);

        var contact = new Contact { Name = "Delete", Email = "del@mail.com", Phone = "(11) 99999-8888", UserId = 1 };
        await repo.AddAsync(contact);

        await repo.ForceDeleteAsync(contact);
        Assert.Empty(context.Contacts);
    }

    [Fact]
    public async void Should_Get_Contact_By_Id_And_By_UserId()
    {
        var context = TestDbContextFactory.Create();
        var repo = new ContactRepository(context);

        var contact = new Contact { Name = "Find", Email = "find@mail.com", Phone = "(11) 99999-8888", UserId = 2 };
        await repo.AddAsync(contact);

        var byId = await repo.GetByIdAsync(contact.Id);
        Assert.Equal("Find", byId!.Name);

        var byUser = (await repo.GetAllByUserIdAsync(2)).First();
        Assert.Equal("Find", byUser.Name);
    }
}
