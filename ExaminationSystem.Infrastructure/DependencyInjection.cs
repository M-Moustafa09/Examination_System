using ExaminationSystem.Application.Interfaces;
using ExaminationSystem.Application.Settings;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using ExaminationSystem.Infrastructure.Persistence;
using ExaminationSystem.Infrastructure.Repositories;
using ExaminationSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ExaminationSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationDbContext(configuration);
        services.AddAuthentication(configuration);
        services.AddMemoryCaching();

        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddScoped(typeof(IGeneralRepository<>), typeof(GeneralRepository<>));

        services.AddScoped<IOtpCachingService, OtpCachingService>();
        services.AddTransient<IEmailSender, EmailService>();

        services.AddScoped<IDiplomaRepository, DiplomaRepository>();
        services.AddScoped<IQuizRepository, QuizRepository>();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }

    private static IServiceCollection AddApplicationDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        return services;
    }

    #region Auth

    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();


        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),

                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings?.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtSettings?.Audience,

                    ValidateLifetime = true
                };
            });

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedEmail = true;
        });


        return services;
    }

    #endregion

    #region Hybrid Caching

    private static IServiceCollection AddMemoryCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();
        return services;
    }

    #endregion
}