using Agenda.Application.Users.Validators;
using Agenda.Application.DTOs;
using FluentValidation.TestHelper;
using Xunit;

namespace Agenda.test.Application.Validators;

public class UserValidatorTests
{
    private readonly UserValidator _validator;

    public UserValidatorTests() => _validator = new UserValidator();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty() =>
        _validator.TestValidate(new UserDto { Name = "" })
                  .ShouldHaveValidationErrorFor(x => x.Name);

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid() =>
        _validator.TestValidate(new UserDto { Email = "invalid" })
                  .ShouldHaveValidationErrorFor(x => x.Email);

    [Fact]
    public void Should_Not_Have_Error_For_Valid_User() =>
        _validator.TestValidate(new UserDto
        {
            Name = "Walber",
            Email = "walber@example.com"
        }).ShouldNotHaveAnyValidationErrors();
}
