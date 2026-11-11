using HotelRitz.Blazor.Data;

namespace HotelRitz.Tests;

public class ApplicationUserTests
{
    [Fact]
    public void ApplicationUser_ShouldHaveName()
    {
        // Arrange
        var user = new ApplicationUser();

        // Act
        user.Name = "Jim";

        // Assert
        Assert.Equal("Jim", user.Name);
    }
}