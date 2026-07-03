using ExaminationSystem.Application.Interfaces;
using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ExaminationSystem.Application.Features.Users.Commands.Login;

public class UserLoginCommandHandler(
    UserManager<ApplicationUser> _userManager,
    SignInManager<ApplicationUser> _signInManager,
    IJwtProvider _jwtProvider,
    ILogger<UserLoginCommandHandler> _logger
)
    : IRequestHandler<UserLoginCommand, Result<UserLoginResponse>>
{
    public async Task<Result<UserLoginResponse>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        #region Check if email exist

        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure<UserLoginResponse>(UserErrors.InvalidCredentials);

        #endregion


        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);


        if (result.Succeeded)
        {
            #region Generate Tokens

            var (token, expiresIn) = _jwtProvider.GenerateToken(user);

            var (refreshToken, refreshTokenExpiration) = _jwtProvider.GenerateRefreshToken();

            #endregion
            // update last login timestamp for admin stats / active users tracking
            user.LastLoginAt = DateTime.UtcNow;
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration
            });

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                _logger.LogError("Refresh token saved successfully for UserId: {UserId}", user.Id);
                return Result.Failure<UserLoginResponse>(UserErrors.UpdateFailed);
            }

            var response = new UserLoginResponse(
                user.Id,
                user.FullName,
                user.Email!, // Email is required for login, so it should not be null here
                token,
                expiresIn * 60,
                refreshToken,
                refreshTokenExpiration
            );

            return Result.Success(response);
        }

        var error = result.IsNotAllowed
            ? UserErrors.EmailNotConfirmed
            : result.IsLockedOut
                ? UserErrors.LockedOut
                : UserErrors.InvalidCredentials;

        return Result.Failure<UserLoginResponse>(error);
    }
}