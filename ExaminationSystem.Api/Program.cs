using ExaminationSystem.Api.Middlewares;
using ExaminationSystem.Application;
using ExaminationSystem.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

#region Controllers

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

#endregion

#region Application & Infrastructure

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

#endregion

#region Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer abcdefghijklmnopqrstuvwxyz\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});

#endregion

#region Middleware

builder.Services.AddScoped<TransactionMiddleware>();

#endregion

#region Serilog

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

#endregion

var app = builder.Build();

#region Pipeline

// Swagger (خليه شغال على طول عشان نتأكد)
app.UseSwagger();
app.UseSwaggerUI();

// HTTPS
app.UseHttpsRedirection();

// Routing
app.UseRouting();

// Authorization
app.UseAuthorization();

// Custom Middleware
app.UseMiddleware<TransactionMiddleware>();

// Controllers
app.MapControllers();

// 🔥 Test Endpoint
app.MapGet("/test", () => "Hello World");

#endregion

app.Run();
