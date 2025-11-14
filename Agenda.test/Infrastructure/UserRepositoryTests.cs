using Agenda.Domain.Entities;
using Agenda.Infrastructure.Repositories;
using Agenda.test.Utils;
using System.Linq;
using Xunit;

namespace Agenda.test.Infrastructure;

public class UserRepositoryTests
{
    [Fact]
    public async void Should_Add_User()
    {
        var context = TestDbContextFactory.Create();
        var repo = new UserRepository(context);

        var user = new User { Name = "Walber", Email = "walber@mail.com" };
        await repo.AddAsync(user);

        var saved = context.Users.First();
        Assert.Equal("Walber", saved.Name);
    }

    [Fact]
    public async void Should_Update_User()
    {
        var context = TestDbContextFactory.Create();
        var repo = new UserRepository(context);

        var user = new User { Name = "Old", Email = "old@mail.com" };
        await repo.AddAsync(user);

        user.Name = "New";
        await repo.UpdateAsync(user);

        var updated = context.Users.First();
        Assert.Equal("New", updated.Name);
    }

    [Fact]
    public async void Should_Delete_And_Restore_User()
    {
        var context = TestDbContextFactory.Create();
        var repo = new UserRepository(context);

        var user = new User { Name = "Temp", Email = "temp@mail.com" };
        await repo.AddAsync(user);

        await repo.DeleteAsync(user);
        Assert.True(user.IsDeleted);

        await repo.RestoreAsync(user);
        Assert.False(user.IsDeleted);
    }

    [Fact]
    public async void Should_Force_Delete_User()
    {
        var context = TestDbContextFactory.Create();
        var repo = new UserRepository(context);

        var user = new User { Name = "Delete", Email = "del@mail.com" };
        await repo.AddAsync(user);

        await repo.ForceDeleteAsync(user);
        Assert.Empty(context.Users);
    }

    [Fact]
    public async void Should_Get_User_By_Id_And_By_Email()
    {
        var context = TestDbContextFactory.Create();
        var repo = new UserRepository(context);

        var user = new User { Name = "Find", Email = "find@mail.com" };
        await repo.AddAsync(user);

        var byId = await repo.GetByIdAsync(user.Id);
        Assert.Equal("Find", byId!.Name);

        var byEmail = await repo.GetByEmailAsync("find@mail.com");
        Assert.Equal("Find", byEmail!.Name);
    }
}
