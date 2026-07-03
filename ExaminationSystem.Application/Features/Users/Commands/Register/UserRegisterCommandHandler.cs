using ExaminationSystem.Application.Interfaces;
using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Errors;
using ExaminationSystem.Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Security.Cryptography;

namespace ExaminationSystem.Application.Features.Users.Commands.Register;

public class UserRegisterCommandHandler(
    UserManager<ApplicationUser> _userManager,
    IOtpCachingService _otpCachingService,
    IEmailSender _emailSender
) : IRequestHandler<UserRegisterCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
    {
        var isEmailExist = _userManager.Users.Any(u => u.Email == request.Email);

        if (isEmailExist)
            return Result.Failure<string>(UserErrors.DuplicateEmail); // Solve OTP rate limit

        var user = new ApplicationUser
        {
            Email = request.Email,
            FullName = request.FullName,
            UserName = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var error = result.Errors.FirstOrDefault();
            return Result.Failure<string>(new Error(error!.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

        _otpCachingService.SetOtp(request.Email, otp);

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            new Dictionary<string, string>
            {
                { "{{name}}", request.FullName },
                { "{{otp_code}}", otp }
            }
        );

        //await _emailSender.SendEmailAsync(request.Email, "✅ Examination System: Email Confirmation", emailBody);


        return Result.Success(user.Id);
    }
}