using ExaminationSystem.Application.Interfaces;
using ExaminationSystem.Application.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace ExaminationSystem.Infrastructure.Services;

public class OtpCachingService(IMemoryCache _memoryCache, IOptions<OtpSettings> _otpSettings) : IOtpCachingService
{
    private readonly OtpSettings _otpSettings = _otpSettings.Value;


    // OTP storage

    public void SetOtp(string key, string otp)
    {
        _memoryCache.Set(Otpkey(key), otp, TimeSpan.FromMinutes(_otpSettings.OtpTtlMinutes));
    }

    public (bool, string?) GetOtp(string key)
    {
        var isExist = _memoryCache.TryGetValue(Otpkey(key), out string? otp);
        return (isExist, otp);
    }

    public void DeleteOtp(string key)
    {
        _memoryCache.Remove(Otpkey(key));
    }

    // Failed attempts 3

    public void IncrementFailedAttempts(string key, TimeSpan ttl)
    {
        var attempts = _memoryCache.GetOrCreate(AttemptsKey(key), entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = ttl;
            return 0;
        });
        _memoryCache.Set(AttemptsKey(key), attempts + 1);
    }

    public void ResetFailedAttempts(string key)
    {
        _memoryCache.Remove(AttemptsKey(key));
    }


    // Lockout
    public void SetLockout(string key, TimeSpan ttl)
    {
        _memoryCache.Set(LockoutKey(key), true, ttl);
    }

    public bool IsLockedOut(string key)
    {
        return _memoryCache.TryGetValue(LockoutKey(key), out _);
    }

    private string Otpkey(string key)
    {
        return $"otp:{key}";
    }

    private string AttemptsKey(string key)
    {
        return $"otp:attempts:{key}";
    }

    private string LockoutKey(string key)
    {
        return $"otp:lockout:{key}";
    }
}