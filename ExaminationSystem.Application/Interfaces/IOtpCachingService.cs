namespace ExaminationSystem.Application.Interfaces;

public interface IOtpCachingService
{
    // OTP storage
    void SetOtp(string key, string otp);
    (bool, string?) GetOtp(string key);
    void DeleteOtp(string key);


    // Failed attempts
    void IncrementFailedAttempts(string key, TimeSpan ttl);
    void ResetFailedAttempts(string key);


    // Lockout
    void SetLockout(string key, TimeSpan ttl);
    bool IsLockedOut(string key);
}