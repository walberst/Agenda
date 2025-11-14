using Agenda.Application.Users.Commands;
using Agenda.Application.DTOs;
using Agenda.Domain.Entities;
using Agenda.Domain.Interfaces;
using AutoMapper;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Agenda.test.Application.Commands;

public class UsersCommandsTests
{
    private readonly Mock<IUserRepository> _repoMock;
    private readonly Mock<IJwtService> _jwtMock;
    private readonly IMapper _mapper;

    public UsersCommandsTests()
    {
        _repoMock = new Mock<IUserRepository>();
        _jwtMock = new Mock<IJwtService>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserDto, User>().ReverseMap();
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task RegisterUser_Should_Add_User()
    {
        var handler = new RegisterUserCommandHandler(_repoMock.Object, _mapper);
        User? saved = null;

        _repoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
                 .Callback<User>(u => saved = u)
                 .Returns(Task.CompletedTask);

        var dto = new UserDto { Name = "Walber", Email = "walber@mail.com" };
        var command = new RegisterUserCommand(dto, "123456");
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(saved);
        Assert.Equal("Walber", saved!.Name);
        Assert.Equal("Walber", result.Name);
    }

    [Fact]
    public async Task AuthenticateUser_Should_Return_Token_When_Valid()
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("123456");
        var user = new User { Id = 1, Email = "walber@mail.com", PasswordHash = passwordHash };
        _repoMock.Setup(r => r.GetByEmailAsync("walber@mail.com")).ReturnsAsync(user);
        _jwtMock.Setup(j => j.GenerateToken(user)).Returns("TOKEN");

        var handler = new AuthenticateUserCommandHandler(_repoMock.Object, _jwtMock.Object);
        var token = await handler.Handle(new AuthenticateUserCommand("walber@mail.com", "123456"), CancellationToken.None);

        Assert.Equal("TOKEN", token);
    }

    [Fact]
    public async Task AuthenticateUser_Should_Return_Empty_When_Invalid()
    {
        _repoMock.Setup(r => r.GetByEmailAsync("unknown@mail.com")).ReturnsAsync((User?)null);
        var handler = new AuthenticateUserCommandHandler(_repoMock.Object, _jwtMock.Object);
        var token = await handler.Handle(new AuthenticateUserCommand("unknown@mail.com", "123456"), CancellationToken.None);

        Assert.Equal(string.Empty, token);
    }

    [Fact]
    public async Task DeleteUser_Should_Return_True_When_Found()
    {
        var existing = new User { Id = 1 };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _repoMock.Setup(r => r.DeleteAsync(existing)).Returns(Task.CompletedTask);

        var handler = new DeleteUserCommandHandler(_repoMock.Object);
        var result = await handler.Handle(new DeleteUserCommand(1), CancellationToken.None);

        Assert.True(result);
        _repoMock.Verify(r => r.DeleteAsync(existing), Times.Once);
    }

    [Fact]
    public async Task DeleteUser_Should_Return_False_When_NotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User?)null);
        var handler = new DeleteUserCommandHandler(_repoMock.Object);
        var result = await handler.Handle(new DeleteUserCommand(1), CancellationToken.None);

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateUser_Should_Update_When_Exists()
    {
        var existing = new User { Id = 1, Name = "Old", Email = "old@mail.com" };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _repoMock.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

        var handler = new UpdateUserCommandHandler(_repoMock.Object, _mapper);
        var dto = new UserDto { Name = "New", Email = "new@mail.com" };
        var result = await handler.Handle(new UpdateUserCommand(1, dto), CancellationToken.None);

        Assert.Equal("New", existing.Name);
        Assert.Equal("New", result!.Name);
    }

    [Fact]
    public async Task UpdateUser_Should_Return_Null_When_NotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User?)null);
        var handler = new UpdateUserCommandHandler(_repoMock.Object, _mapper);
        var result = await handler.Handle(new UpdateUserCommand(1, new UserDto()), CancellationToken.None);

        Assert.Null(result);
    }
}
