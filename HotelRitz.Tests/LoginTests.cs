
using Bunit;
using Moq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HotelRitz.Blazor.Data;
using HotelRitz.Blazor.Components.Account;
using HotelRitz.Blazor.Components.Account.Pages;

namespace HotelRitz.Tests;

public class LoginTests
{
    [Fact]
    public async Task LoginUser_WithWrongPassword_ShowsErrorMessage()
    {
        // Arrange

        var userStore = new Mock<IUserStore<ApplicationUser>>();

        var userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStore.Object,
            Options.Create(new IdentityOptions()),
            new PasswordHasher<ApplicationUser>(),
            Array.Empty<IUserValidator<ApplicationUser>>(),
            Array.Empty<IPasswordValidator<ApplicationUser>>(),
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            new ServiceCollection().BuildServiceProvider(),
            Mock.Of<ILogger<UserManager<ApplicationUser>>>()
        );

        var authenticationSchemeProvider =
            new Mock<IAuthenticationSchemeProvider>();

        authenticationSchemeProvider
            .Setup(x => x.GetAllSchemesAsync())
            .ReturnsAsync(Array.Empty<AuthenticationScheme>());

        var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            userManagerMock.Object,
            new HttpContextAccessor(),
            Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
            Options.Create(new IdentityOptions()),
            Mock.Of<ILogger<SignInManager<ApplicationUser>>>(),
            authenticationSchemeProvider.Object,
            Mock.Of<IUserConfirmation<ApplicationUser>>()
        );

        signInManagerMock
            .Setup(x => x.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
            .ReturnsAsync(SignInResult.Failed);

        // Opret bUnit testkontekst
        using var ctx = new BunitContext();

        ctx.Services.AddLogging();

        ctx.Services.AddSingleton(userManagerMock.Object);
        ctx.Services.AddSingleton(signInManagerMock.Object);

        ctx.Services.AddScoped<IdentityRedirectManager>(
            provider => new IdentityRedirectManager(
                provider.GetRequiredService<NavigationManager>()));

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = "POST";

        // Render Login-komponenten
        var cut = ctx.Render<Login>(parameters =>
            parameters.AddCascadingValue(httpContext));

        // Indtast loginoplysninger
        cut.Find("#Input\\.Email").Change("test@example.com");
        cut.Find("#Input\\.Password").Change("wrong-password");

        // Act
        await cut.Find("form").TriggerEventAsync("onsubmit", new EventArgs());

        // Assert
        Assert.Contains("Error: Invalid login attempt.", cut.Markup);

        signInManagerMock.Verify(
            x => x.PasswordSignInAsync(
                "test@example.com",
                "wrong-password",
                false,
                false),
            Times.Once);
    }
    
    
    
    [Fact]
    public async Task LoginUser_WithCorrectPassword_RedirectsUser()
    {
        // Arrange

        var userStore = new Mock<IUserStore<ApplicationUser>>();

        var userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStore.Object,
            Options.Create(new IdentityOptions()),
            new PasswordHasher<ApplicationUser>(),
            Array.Empty<IUserValidator<ApplicationUser>>(),
            Array.Empty<IPasswordValidator<ApplicationUser>>(),
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            new ServiceCollection().BuildServiceProvider(),
            Mock.Of<ILogger<UserManager<ApplicationUser>>>()
        );

        var authenticationSchemeProvider =
            new Mock<IAuthenticationSchemeProvider>();

        authenticationSchemeProvider
            .Setup(x => x.GetAllSchemesAsync())
            .ReturnsAsync(Array.Empty<AuthenticationScheme>());

        var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            userManagerMock.Object,
            new HttpContextAccessor(),
            Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
            Options.Create(new IdentityOptions()),
            Mock.Of<ILogger<SignInManager<ApplicationUser>>>(),
            authenticationSchemeProvider.Object,
            Mock.Of<IUserConfirmation<ApplicationUser>>()
        );

        // Simuler et vellykket login
        signInManagerMock
            .Setup(x => x.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
            .ReturnsAsync(SignInResult.Success);

        using var ctx = new BunitContext();

        ctx.Services.AddLogging();

        ctx.Services.AddSingleton(userManagerMock.Object);
        ctx.Services.AddSingleton(signInManagerMock.Object);

        ctx.Services.AddScoped<IdentityRedirectManager>(
            provider => new IdentityRedirectManager(
                provider.GetRequiredService<NavigationManager>()));

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = "POST";

        var cut = ctx.Render<Login>(parameters =>
            parameters.AddCascadingValue(httpContext));

        // Indtast loginoplysninger
        cut.Find("#Input\\.Email").Change("test@example.com");
        cut.Find("#Input\\.Password").Change("correct-password");

        // Act
        await cut.Find("form").TriggerEventAsync("onsubmit", new EventArgs());

        // Assert
        Assert.DoesNotContain("Error: Invalid login attempt.", cut.Markup);

        signInManagerMock.Verify(
            x => x.PasswordSignInAsync(
                "test@example.com",
                "correct-password",
                false,
                false),
            Times.Once);
    }

    
    
    
[Fact]
public async Task LoginUser_WithLockedAccount_RedirectsToLockout()
{
    // Arrange
    var userStore = new Mock<IUserStore<ApplicationUser>>();

    var userManagerMock = new Mock<UserManager<ApplicationUser>>(
        userStore.Object,
        Options.Create(new IdentityOptions()),
        new PasswordHasher<ApplicationUser>(),
        Array.Empty<IUserValidator<ApplicationUser>>(),
        Array.Empty<IPasswordValidator<ApplicationUser>>(),
        new UpperInvariantLookupNormalizer(),
        new IdentityErrorDescriber(),
        new ServiceCollection().BuildServiceProvider(),
        Mock.Of<ILogger<UserManager<ApplicationUser>>>()
    );

    var authenticationSchemeProvider =
        new Mock<IAuthenticationSchemeProvider>();

    authenticationSchemeProvider
        .Setup(x => x.GetAllSchemesAsync())
        .ReturnsAsync(Array.Empty<AuthenticationScheme>());

    var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
        userManagerMock.Object,
        new HttpContextAccessor(),
        Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
        Options.Create(new IdentityOptions()),
        Mock.Of<ILogger<SignInManager<ApplicationUser>>>(),
        authenticationSchemeProvider.Object,
        Mock.Of<IUserConfirmation<ApplicationUser>>()
    );

    signInManagerMock
        .Setup(x => x.PasswordSignInAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>()))
        .ReturnsAsync(SignInResult.LockedOut);

    using var ctx = new BunitContext();

    ctx.Services.AddLogging();
    ctx.Services.AddSingleton(userManagerMock.Object);
    ctx.Services.AddSingleton(signInManagerMock.Object);

    ctx.Services.AddScoped<IdentityRedirectManager>(
        provider => new IdentityRedirectManager(
            provider.GetRequiredService<NavigationManager>()));

    var httpContext = new DefaultHttpContext();
    httpContext.Request.Method = "POST";

    var cut = ctx.Render<Login>(parameters =>
        parameters.AddCascadingValue(httpContext));

    cut.Find("#Input\\.Email").Change("test@example.com");
    cut.Find("#Input\\.Password").Change("password123");

    // Act
    await cut.Find("form").TriggerEventAsync("onsubmit", new EventArgs());

    // Assert
    Assert.Equal(
        "http://localhost/Account/Lockout",
        ctx.Services.GetRequiredService<NavigationManager>().Uri
    );

    signInManagerMock.Verify(
        x => x.PasswordSignInAsync(
            "test@example.com",
            "password123",
            false,
            false),
        Times.Once);
}




[Fact]
public async Task LoginUser_WhenTwoFactorRequired_RedirectsToTwoFactor()
{
    // Arrange
    var userStore = new Mock<IUserStore<ApplicationUser>>();

    var userManagerMock = new Mock<UserManager<ApplicationUser>>(
        userStore.Object,
        Options.Create(new IdentityOptions()),
        new PasswordHasher<ApplicationUser>(),
        Array.Empty<IUserValidator<ApplicationUser>>(),
        Array.Empty<IPasswordValidator<ApplicationUser>>(),
        new UpperInvariantLookupNormalizer(),
        new IdentityErrorDescriber(),
        new ServiceCollection().BuildServiceProvider(),
        Mock.Of<ILogger<UserManager<ApplicationUser>>>()
    );

    var authenticationSchemeProvider =
        new Mock<IAuthenticationSchemeProvider>();

    authenticationSchemeProvider
        .Setup(x => x.GetAllSchemesAsync())
        .ReturnsAsync(Array.Empty<AuthenticationScheme>());

    var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
        userManagerMock.Object,
        new HttpContextAccessor(),
        Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
        Options.Create(new IdentityOptions()),
        Mock.Of<ILogger<SignInManager<ApplicationUser>>>(),
        authenticationSchemeProvider.Object,
        Mock.Of<IUserConfirmation<ApplicationUser>>()
    );

    signInManagerMock
        .Setup(x => x.PasswordSignInAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>()))
        .ReturnsAsync(SignInResult.TwoFactorRequired);

    using var ctx = new BunitContext();

    ctx.Services.AddLogging();
    ctx.Services.AddSingleton(userManagerMock.Object);
    ctx.Services.AddSingleton(signInManagerMock.Object);

    ctx.Services.AddScoped<IdentityRedirectManager>(
        provider => new IdentityRedirectManager(
            provider.GetRequiredService<NavigationManager>()));

    var httpContext = new DefaultHttpContext();
    httpContext.Request.Method = "POST";

    var cut = ctx.Render<Login>(parameters =>
        parameters.AddCascadingValue(httpContext));

    cut.Find("#Input\\.Email").Change("test@example.com");
    cut.Find("#Input\\.Password").Change("password123");

    // Act
    await cut.Find("form").TriggerEventAsync("onsubmit", new EventArgs());

    // Assert
    var navigationManager =
        ctx.Services.GetRequiredService<NavigationManager>();

    Assert.Contains(
        "Account/LoginWith2fa",
        navigationManager.Uri
    );

    signInManagerMock.Verify(
        x => x.PasswordSignInAsync(
            "test@example.com",
            "password123",
            false,
            false),
        Times.Once);
}



[Fact]
public async Task LoginUser_WithEmptyEmail_DoesNotAttemptLogin()
{
    // Arrange
    var userStore = new Mock<IUserStore<ApplicationUser>>();

    var userManagerMock = new Mock<UserManager<ApplicationUser>>(
        userStore.Object,
        Options.Create(new IdentityOptions()),
        new PasswordHasher<ApplicationUser>(),
        Array.Empty<IUserValidator<ApplicationUser>>(),
        Array.Empty<IPasswordValidator<ApplicationUser>>(),
        new UpperInvariantLookupNormalizer(),
        new IdentityErrorDescriber(),
        new ServiceCollection().BuildServiceProvider(),
        Mock.Of<ILogger<UserManager<ApplicationUser>>>()
    );

    var authenticationSchemeProvider =
        new Mock<IAuthenticationSchemeProvider>();

    authenticationSchemeProvider
        .Setup(x => x.GetAllSchemesAsync())
        .ReturnsAsync(Array.Empty<AuthenticationScheme>());

    var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
        userManagerMock.Object,
        new HttpContextAccessor(),
        Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
        Options.Create(new IdentityOptions()),
        Mock.Of<ILogger<SignInManager<ApplicationUser>>>(),
        authenticationSchemeProvider.Object,
        Mock.Of<IUserConfirmation<ApplicationUser>>()
    );

    using var ctx = new BunitContext();

    ctx.Services.AddLogging();
    ctx.Services.AddSingleton(userManagerMock.Object);
    ctx.Services.AddSingleton(signInManagerMock.Object);

    ctx.Services.AddScoped<IdentityRedirectManager>(
        provider => new IdentityRedirectManager(
            provider.GetRequiredService<NavigationManager>()));

    var httpContext = new DefaultHttpContext();
    httpContext.Request.Method = "POST";

    var cut = ctx.Render<Login>(parameters =>
        parameters.AddCascadingValue(httpContext));

    // Email er tom
    cut.Find("#Input\\.Password").Change("password123");

    // Act
    await cut.Find("form").TriggerEventAsync("onsubmit", new EventArgs());

    // Assert
    signInManagerMock.Verify(
        x => x.PasswordSignInAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>()),
        Times.Never);
}



[Fact]
public async Task LoginUser_WithInvalidEmail_DoesNotAttemptLogin()
{
    // Arrange
    var userStore = new Mock<IUserStore<ApplicationUser>>();

    var userManagerMock = new Mock<UserManager<ApplicationUser>>(
        userStore.Object,
        Options.Create(new IdentityOptions()),
        new PasswordHasher<ApplicationUser>(),
        Array.Empty<IUserValidator<ApplicationUser>>(),
        Array.Empty<IPasswordValidator<ApplicationUser>>(),
        new UpperInvariantLookupNormalizer(),
        new IdentityErrorDescriber(),
        new ServiceCollection().BuildServiceProvider(),
        Mock.Of<ILogger<UserManager<ApplicationUser>>>()
    );

    var authenticationSchemeProvider =
        new Mock<IAuthenticationSchemeProvider>();

    authenticationSchemeProvider
        .Setup(x => x.GetAllSchemesAsync())
        .ReturnsAsync(Array.Empty<AuthenticationScheme>());

    var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
        userManagerMock.Object,
        new HttpContextAccessor(),
        Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
        Options.Create(new IdentityOptions()),
        Mock.Of<ILogger<SignInManager<ApplicationUser>>>(),
        authenticationSchemeProvider.Object,
        Mock.Of<IUserConfirmation<ApplicationUser>>()
    );

    using var ctx = new BunitContext();

    ctx.Services.AddLogging();
    ctx.Services.AddSingleton(userManagerMock.Object);
    ctx.Services.AddSingleton(signInManagerMock.Object);

    ctx.Services.AddScoped<IdentityRedirectManager>(
        provider => new IdentityRedirectManager(
            provider.GetRequiredService<NavigationManager>()));

    var httpContext = new DefaultHttpContext();
    httpContext.Request.Method = "POST";

    var cut = ctx.Render<Login>(parameters =>
        parameters.AddCascadingValue(httpContext));

    // Indtast ugyldig email og adgangskode
    cut.Find("#Input\\.Email").Change("ikke-en-email");
    cut.Find("#Input\\.Password").Change("password123");

    // Act
    await cut.Find("form").TriggerEventAsync("onsubmit", new EventArgs());

    // Assert
    signInManagerMock.Verify(
        x => x.PasswordSignInAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>()),
        Times.Never);
}



[Fact]
public async Task LoginUser_WithEmptyPassword_DoesNotAttemptLogin()
{
    // Arrange
    var userStore = new Mock<IUserStore<ApplicationUser>>();

    var userManagerMock = new Mock<UserManager<ApplicationUser>>(
        userStore.Object,
        Options.Create(new IdentityOptions()),
        new PasswordHasher<ApplicationUser>(),
        Array.Empty<IUserValidator<ApplicationUser>>(),
        Array.Empty<IPasswordValidator<ApplicationUser>>(),
        new UpperInvariantLookupNormalizer(),
        new IdentityErrorDescriber(),
        new ServiceCollection().BuildServiceProvider(),
        Mock.Of<ILogger<UserManager<ApplicationUser>>>()
    );

    var authenticationSchemeProvider =
        new Mock<IAuthenticationSchemeProvider>();

    authenticationSchemeProvider
        .Setup(x => x.GetAllSchemesAsync())
        .ReturnsAsync(Array.Empty<AuthenticationScheme>());

    var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
        userManagerMock.Object,
        new HttpContextAccessor(),
        Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
        Options.Create(new IdentityOptions()),
        Mock.Of<ILogger<SignInManager<ApplicationUser>>>(),
        authenticationSchemeProvider.Object,
        Mock.Of<IUserConfirmation<ApplicationUser>>()
    );

    using var ctx = new BunitContext();

    ctx.Services.AddLogging();
    ctx.Services.AddSingleton(userManagerMock.Object);
    ctx.Services.AddSingleton(signInManagerMock.Object);

    ctx.Services.AddScoped<IdentityRedirectManager>(
        provider => new IdentityRedirectManager(
            provider.GetRequiredService<NavigationManager>()));

    var httpContext = new DefaultHttpContext();
    httpContext.Request.Method = "POST";

    var cut = ctx.Render<Login>(parameters =>
        parameters.AddCascadingValue(httpContext));

    // Indtast email, men ingen adgangskode
    cut.Find("#Input\\.Email").Change("test@example.com");

    // Act
    await cut.Find("form").TriggerEventAsync("onsubmit", new EventArgs());

    // Assert
    signInManagerMock.Verify(
        x => x.PasswordSignInAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>()),
        Times.Never);
}


}
