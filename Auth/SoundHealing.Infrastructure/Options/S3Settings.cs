namespace SoundHealing.Infrastructure.Options;

public sealed class S3Settings
{
    public string AccessKey { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public string Region { get; init; } = string.Empty;
    public string MeditationsBucketName { get; init; } = string.Empty;
}