namespace ExaminationSystem.Application.Features.Users.Commands.Login;

public record UserLoginResponse(
    string UserId,
    string FullName,
    string Email,
    string AccessToken,
    int AccessTokenExpiresInBySeconds,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt
);