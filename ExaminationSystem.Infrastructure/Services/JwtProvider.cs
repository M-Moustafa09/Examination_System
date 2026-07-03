using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ExaminationSystem.Application.Interfaces;
using ExaminationSystem.Application.Settings;
using ExaminationSystem.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ExaminationSystem.Infrastructure.Services;

public class JwtProvider(IOptions<JwtSettings> options, ILogger<JwtProvider> _logger) : IJwtProvider
{
    private readonly JwtSettings _options = options.Value;

    private int _refreshTokenExpiryDays => 7;

    public (string token, DateTime expiresAt) GenerateRefreshToken()
    {
        _logger.LogDebug("Refresh token generated successfully");

        return (Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            DateTime.UtcNow.AddDays(_refreshTokenExpiryDays));
    }

    public (string token, int expiresIn) GenerateToken(ApplicationUser applicationUser)
    {
        _logger.LogDebug(
            "Generating JWT token for user {UserId}",
            applicationUser.Id);

        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, applicationUser.Id),
            new(JwtRegisteredClaimNames.Name, applicationUser.FullName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            //new (nameof(roles) , JsonSerializer.Serialize(roles) , JsonClaimValueTypes.JsonArray ),
            //new (nameof(permissions) , JsonSerializer.Serialize(permissions) , JsonClaimValueTypes.JsonArray ),
        ];

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
            signingCredentials: signingCredentials
        );

        _logger.LogInformation(
            "JWT token generated successfully for user {UserId}. Expires in {ExpiryMinutes} minutes",
            applicationUser.Id,
            _options.ExpiryMinutes);


        return (
            new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            _options.ExpiryMinutes * 60
        );
    }

    public string? ValidateToken(string token)
    {
        _logger.LogDebug("Validating JWT token");


        var tokenHandler = new JwtSecurityTokenHandler();
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = symmetricSecurityKey,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var userId = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;

            _logger.LogDebug(
                "JWT token validated successfully for user {UserId}",
                userId);

            return userId;
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogWarning(
                ex,
                "JWT token validation failed — invalid token");

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error during JWT token validation");

            return null;
        }
    }
}