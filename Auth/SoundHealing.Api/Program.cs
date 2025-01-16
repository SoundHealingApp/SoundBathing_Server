using Auth.Application.Contracts.Requests.Auth;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SoundHealing.Application.Interfaces;
using SoundHealing.Application.Validators;
using SoundHealing.Core.Interfaces;
using SoundHealing.Extensions;
using SoundHealing.Infrastructure;
using SoundHealing.Infrastructure.Helpers;
using SoundHealing.Infrastructure.Options;
using SoundHealing.Infrastructure.Providers;
using SoundHealing.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
{
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
}

builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddScoped<IUserCredentialsRepository, UserCredentialsRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMediationRepository, MeditationRepository>();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddApiAuthentication(builder.Configuration);
builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();

builder.Services.AddDbContext<UserDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(UserDbContext)));
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();