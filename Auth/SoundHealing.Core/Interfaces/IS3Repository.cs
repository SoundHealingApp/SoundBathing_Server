using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;

namespace SoundHealing.Core.Interfaces;

public interface IS3Repository
{
    public Task<string?> AddMeditationImageAsync(IFormFile file, Guid meditationId);

    public Task<string?> AddMeditationAudioAsync(IFormFile file, Guid meditationId);
    
    public Task<GetObjectResponse?> GetMeditationImageAsync(Guid meditationId);
    
    public Task<GetObjectResponse?> GetMeditationAudioAsync(Guid meditationId);
}