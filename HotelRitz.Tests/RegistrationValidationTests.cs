using HotelRitz.Blazor.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace HotelRitz.Tests;

public class RegistrationValidationTests
{
    
    private UserManager<ApplicationUser> CreateUserManager()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();

        var options = Options.Create(new IdentityOptions());

        return new UserManager<ApplicationUser>(
            store.Object,
            options,
            new PasswordHasher<ApplicationUser>(),
            Array.Empty<IUserValidator<ApplicationUser>>(),
            Array.Empty<IPasswordValidator<ApplicationUser>>(),
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            null!,
            Mock.Of<ILogger<UserManager<ApplicationUser>>>()
        );
    }
    
    
    [Fact]
    public async Task Password_ShouldRejectTooShortPassword()
    {
        // Arrange
        var userManager = CreateUserManager();
        var validator = new PasswordValidator<ApplicationUser>();
        var user = new ApplicationUser();

        // Act
        var result = await validator.ValidateAsync(
            userManager, user, "123");

        // Assert
        Assert.False(result.Succeeded);
    }
    
    [Fact]
    public async Task Password_ShouldApprove()
    {
        // Arrange
        var userManager = CreateUserManager();
        var validator = new PasswordValidator<ApplicationUser>();
        var user = new ApplicationUser();

        // Act
        var result = await validator.ValidateAsync(
            userManager, user, "MikkelErGrim123!");

        // Assert
        Assert.True(result.Succeeded);
    }
    
    [Fact]
    public void ConfirmPassword_ShouldRejectDifferentPasswords()
    {
        // Arrange
        var compareValidator = new CompareAttribute("Password");
        var password = "MikkelErGrim123!";
        var confirmPassword = "MikkelErGrim456!";

        var context = new ValidationContext(new
        {
            Password = password
        });

        // Act
        var result = compareValidator.GetValidationResult(
            confirmPassword, context);

        // Assert
        Assert.NotEqual(ValidationResult.Success, result);
    }
    
    [Fact]
    public void Email_ShouldRejectInvalidEmail()
    {
        // Arrange
        var emailValidator = new EmailAddressAttribute();
        var email = "ikke-en-email";

        // Act
        var result = emailValidator.IsValid(email);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void Email_ShouldRejectEmptyEmail()
    {
        // Arrange
        var requiredValidator = new RequiredAttribute();
        var email = "";

        // Act
        var result = requiredValidator.IsValid(email);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void Email_ShouldApprove()
    {
        // Arrange
        var emailValidator = new EmailAddressAttribute();
        var email = "mikkelKofoed@gmail.com";

        // Act
        var result = emailValidator.IsValid(email);

        // Assert
        Assert.True(result);
    }
}