using ExaminationSystem.Application.Behaviors;
using ExaminationSystem.Application.Settings;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace ExaminationSystem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentValidation();
        services.AddMediatR();

        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<OtpSettings>()
            .Bind(configuration.GetSection(OtpSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<MailSettings>()
            .Bind(configuration.GetSection(MailSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }

    private static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddFluentValidationAutoValidation();
        return services;
    }

    #region MediatR

    private static void AddMediatR(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }

    #endregion
}