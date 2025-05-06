using Amazon;
using Amazon.S3;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FluentValidation;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Options;
using SoundHealing.Application.Contracts.Requests.Auth;
using SoundHealing.Application.Contracts.Requests.Meditation;
using SoundHealing.Application.Interfaces;
using SoundHealing.Application.Validators.Auth;
using SoundHealing.Application.Validators.Meditation;
using SoundHealing.Core.Interfaces;
using SoundHealing.Extensions;
using SoundHealing.Infrastructure;
using SoundHealing.Infrastructure.Helpers;
using SoundHealing.Infrastructure.Options;
using SoundHealing.Infrastructure.Providers;
using SoundHealing.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// var keyVaultUrl = configuration.GetSection("KeyVault:KeyVaultURL");
// var keyVaultClient = new KeyVaultClient(
//     new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider().KeyVaultTokenCallback));
// configuration.AddAzureKeyVault(keyVaultUrl.Value!, new DefaultKeyVaultSecretManager());
// var client = new SecretClient(new Uri(keyVaultUrl.Value!), new DefaultAzureCredential());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
{
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
}

builder.Services.AddScoped<IUserCredentialsRepository, UserCredentialsRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMediationRepository, MeditationRepository>();
builder.Services.AddScoped<IS3Repository, S3Repository>();
builder.Services.AddScoped<IMeditationFeedbackRepository, MeditationFeedbackRepository>();
builder.Services.AddScoped<ILiveStreamRepository, LiveStreamRepository>();
builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddApiAuthentication(builder.Configuration, builder.Environment);
builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();
builder.Services.AddScoped<IValidator<ChangeCredentialsRequest>, ChangeCredentialsRequestValidator>();
builder.Services.AddScoped<IValidator<AddMeditationFeedbackRequest>, AddMeditationFeedbackRequestValidator>();
builder.Services.AddScoped<IValidator<ChangeMeditationFeedbackRequest>, ChangeFeedbackRequestValidator>();

builder.Services.Configure<S3Settings>(builder.Configuration.GetSection(nameof(S3Settings)));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<IAmazonS3>(sp =>
    {
        var s3Settings = sp.GetRequiredService<IOptions<S3Settings>>().Value;
        var config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.GetBySystemName(s3Settings.Region),
        };
        return new AmazonS3Client(s3Settings.AccessKey, s3Settings.SecretKey, config);
    });
} 
else if (builder.Environment.IsProduction())
{
    builder.Services.AddSingleton<IAmazonS3>(sp =>
    {
        var s3Settings = sp.GetRequiredService<IOptions<S3Settings>>().Value;
        var keyVaultUrl = configuration.GetSection("KeyVault:KeyVaultURL");
        var keyVaultClient = new KeyVaultClient(
            new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider().KeyVaultTokenCallback));
        configuration.AddAzureKeyVault(keyVaultUrl.Value!, new DefaultKeyVaultSecretManager());
        var client = new SecretClient(new Uri(keyVaultUrl.Value!), new DefaultAzureCredential());
        var s3AccessKey = client.GetSecret("S3AccessKey").Value.Value!;
        var s3SecretKey = client.GetSecret("S3SecretKey").Value.Value!;

        var config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.GetBySystemName(s3Settings.Region),
        };
        return new AmazonS3Client(s3AccessKey, s3SecretKey, config);
    });
}

var keyVaultUrl = configuration.GetSection("KeyVault:KeyVaultURL");
var keyVaultClient = new KeyVaultClient(
    new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider().KeyVaultTokenCallback));
configuration.AddAzureKeyVault(keyVaultUrl.Value!, new DefaultKeyVaultSecretManager());
var client = new SecretClient(new Uri(keyVaultUrl.Value!), new DefaultAzureCredential());
builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        options.UseNpgsql(client.GetSecret("ConnectionString").Value.Value!);
    });

// if (builder.Environment.IsDevelopment())
// {
//     builder.Services.AddDbContext<AppDbContext>(
//         options =>
//         {
//             options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(AppDbContext)));
//         });
// } 
// else if (builder.Environment.IsProduction())
// {
//     var keyVaultUrl = configuration.GetSection("KeyVault:KeyVaultURL");
//     var keyVaultClient = new KeyVaultClient(
//         new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider().KeyVaultTokenCallback));
//     configuration.AddAzureKeyVault(keyVaultUrl.Value!, new DefaultKeyVaultSecretManager());
//     var client = new SecretClient(new Uri(keyVaultUrl.Value!), new DefaultAzureCredential());
//     builder.Services.AddDbContext<AppDbContext>(
//         options =>
//         {
//             options.UseNpgsql(builder.Configuration.GetConnectionString(client.GetSecret("ConnectionString").Value.Value!));
//         });
// }

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();