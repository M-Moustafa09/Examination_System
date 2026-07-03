using ExaminationSystem.Domain.Entities;

namespace ExaminationSystem.Application.Interfaces;

public interface IJwtProvider
{
    (string token, int expiresIn) GenerateToken(ApplicationUser applicationUser);
    string? ValidateToken(string token);

    (string token, DateTime expiresAt) GenerateRefreshToken();
}