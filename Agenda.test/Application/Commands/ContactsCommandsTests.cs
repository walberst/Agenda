using Agenda.Application.Contacts.Commands;
using Agenda.Application.DTOs;
using Agenda.Domain.Entities;
using Agenda.Domain.Interfaces;
using AutoMapper;
using FluentValidation;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Agenda.test.Application.Commands;

public class ContactsCommandsTests
{
    private readonly Mock<IContactRepository> _repoMock;
    private readonly IMapper _mapper;

    public ContactsCommandsTests()
    {
        _repoMock = new Mock<IContactRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ContactDto, Contact>().ReverseMap();
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task CreateContact_Should_Add_Contact()
    {
        var handler = new CreateContactCommandHandler(_repoMock.Object, _mapper);
        var dto = new ContactDto { Name = "Walber", Email = "walber@mail.com", Phone = "(11) 98765-4321", UserId = 1 };
        Contact? saved = null;

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Contact>()))
                 .Callback<Contact>(c => saved = c)
                 .Returns(Task.CompletedTask);

        var result = await handler.Handle(new CreateContactCommand(dto), CancellationToken.None);

        Assert.NotNull(saved);
        Assert.Equal("Walber", saved!.Name);
        Assert.Equal("Walber", result.Name);
    }

    [Fact]
    public async Task UpdateContact_Should_Update_Contact_When_Exists()
    {
        var existing = new Contact { Id = 1, Name = "Old", Email = "old@mail.com", Phone = "(11) 99999-8888", UserId = 1 };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _repoMock.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

        var handler = new UpdateContactCommandHandler(_repoMock.Object, _mapper);
        var dto = new ContactDto { Name = "New", Email = "new@mail.com", Phone = "(11) 98765-4321", UserId = 1 };
        var result = await handler.Handle(new UpdateContactCommand(1, dto), CancellationToken.None);

        Assert.Equal("New", existing.Name);
        Assert.Equal("New", result!.Name);
    }

    [Fact]
    public async Task UpdateContact_Should_Return_Null_When_NotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Contact?)null);
        var handler = new UpdateContactCommandHandler(_repoMock.Object, _mapper);
        var result = await handler.Handle(new UpdateContactCommand(1, new ContactDto()), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteContact_Should_Return_True_When_Found()
    {
        var existing = new Contact { Id = 1 };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _repoMock.Setup(r => r.DeleteAsync(existing)).Returns(Task.CompletedTask);

        var handler = new DeleteContactCommandHandler(_repoMock.Object);
        var result = await handler.Handle(new DeleteContactCommand(1), CancellationToken.None);

        Assert.True(result);
        _repoMock.Verify(r => r.DeleteAsync(existing), Times.Once);
    }

    [Fact]
    public async Task DeleteContact_Should_Return_False_When_NotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Contact?)null);
        var handler = new DeleteContactCommandHandler(_repoMock.Object);
        var result = await handler.Handle(new DeleteContactCommand(1), CancellationToken.None);

        Assert.False(result);
    }
}
