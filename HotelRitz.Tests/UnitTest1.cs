namespace HotelRitz.Tests;

public class UnitTest1
{
    [Fact]
    public void Login_WithValidCredentials_ReturnsTrue()
    {
        // Arrange
        string email = "test@test.dk";
        string password = "Password123!";

        // Act
        var result = Login(email, password);

        // Assert
        Assert.True(result);
    }
    
    
}
