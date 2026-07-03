namespace ExaminationSystem.Application.Settings;

public class OtpSettings
{
    public static string SectionName { get; set; } = "OtpSettings";

    public int OtpLength { get; set; } = 6;

    public int OtpTtlMinutes { get; set; } = 15;
}