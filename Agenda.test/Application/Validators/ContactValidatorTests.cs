using Agenda.Application.Contacts.Validators;
using Agenda.Application.DTOs;
using FluentValidation.TestHelper;
using Xunit;

namespace Agenda.test.Application.Validators;

public class ContactValidatorTests
{
    private readonly ContactValidator _validator;

    public ContactValidatorTests() => _validator = new ContactValidator();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty() =>
        _validator.TestValidate(new ContactDto { Name = "" })
                  .ShouldHaveValidationErrorFor(x => x.Name);

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid() =>
        _validator.TestValidate(new ContactDto { Email = "invalid" })
                  .ShouldHaveValidationErrorFor(x => x.Email);

    [Fact]
    public void Should_Have_Error_When_Phone_Is_Invalid() =>
        _validator.TestValidate(new ContactDto { Phone = "123" })
                  .ShouldHaveValidationErrorFor(x => x.Phone);

    [Fact]
    public void Should_Have_Error_When_UserId_Is_Invalid() =>
        _validator.TestValidate(new ContactDto { UserId = 0 })
                  .ShouldHaveValidationErrorFor(x => x.UserId);

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Contact() =>
        _validator.TestValidate(new ContactDto
        {
            Name = "Walber",
            Email = "walber@exemplo.com",
            Phone = "(11) 98765-4321",
            UserId = 1
        }).ShouldNotHaveAnyValidationErrors();
}
