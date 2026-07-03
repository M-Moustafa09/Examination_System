using ExaminationSystem.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ExaminationSystem.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string Id => httpContextAccessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
}
