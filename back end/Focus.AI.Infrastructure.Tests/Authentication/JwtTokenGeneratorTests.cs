using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using Focus.AI.Domain.Entities;
using Focus.AI.Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Moq;

namespace Focus.AI.Infrastructure.Tests.Authentication;

public class JwtTokenGeneratorTests
{
    [Fact]
    public void GenerateToken_WithValidUser_ReturnsValidJwt()
    {
        // Arrange
        var settings = new JwtSettings
        {
            Secret = "SuperSecretKeyFromAppSettingsThatIsVeryLongAndSecureForFocusAI2026!",
            ExpiryMinutes = 60,
            Issuer = "TestIssuer",
            Audience = "TestAudience"
        };

        var optionsMock = new Mock<IOptions<JwtSettings>>();
        optionsMock.Setup(x => x.Value).Returns(settings);

        var generator = new JwtTokenGenerator(optionsMock.Object);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@focus.ai",
            Role = new Role { Name = "Admin" }
        };

        // Act
        var token = generator.GenerateToken(user);

        // Assert
        token.Should().NotBeNullOrEmpty();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Issuer.Should().Be(settings.Issuer);
        jwtToken.Audiences.Should().Contain(settings.Audience);
        jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value.Should().Be(user.Id.ToString());
        jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value.Should().Be(user.Email);
        jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value.Should().Be("Admin");
    }
}
