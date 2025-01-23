using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SoundHealing.Core.Interfaces;
using SoundHealing.Infrastructure.Options;

namespace SoundHealing.Infrastructure.Repositories;

public class S3Repository(IAmazonS3 s3Client, IOptions<S3Settings> s3Settings) : IS3Repository
{
    public async Task<string?> AddMeditationImageAsync(IFormFile file, Guid meditationId)
    {
        var key = GetMeditationImageKey(meditationId);
        
        var isFileUploaded = await AddFileAsync(file, key);
        
        return isFileUploaded ? key : null;
    }
    
    public async Task<string?> AddMeditationAudioAsync(IFormFile file, Guid meditationId)
    {
        var key = GetMeditationAudioKey(meditationId);
        
        var isFileUploaded = await AddFileAsync(file, key);
        
        return isFileUploaded ? key : null;
    }

    public async Task<GetObjectResponse?> GetMeditationImageAsync(Guid meditationId)
    {
        try
        {
            var getRequest = new GetObjectRequest
            {
                BucketName = s3Settings.Value.MeditationsBucketName,
                Key = GetMeditationImageKey(meditationId)
            };
        
            return await s3Client.GetObjectAsync(getRequest);
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<GetObjectResponse?> GetMeditationAudioAsync(Guid meditationId)
    {
        try
        {
            var getRequest = new GetObjectRequest
            {
                BucketName = s3Settings.Value.MeditationsBucketName,
                Key = GetMeditationAudioKey(meditationId)
            };
            
            return await s3Client.GetObjectAsync(getRequest);
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    private async Task<bool> AddFileAsync(IFormFile file, string key)
    {
        try
        {
            await using var stream = file.OpenReadStream();
        
            var putRequest = new PutObjectRequest
            {
                BucketName = s3Settings.Value.MeditationsBucketName,
                Key = key,
                InputStream = stream,
                ContentType = file.ContentType,
                Metadata =
                {
                    ["file-name"] = file.FileName
                }
            };
        
            await s3Client.PutObjectAsync(putRequest);
            
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static string GetMeditationImageKey(Guid meditationId)
    {
        return $"images/{meditationId}";
    }
    
    private static string GetMeditationAudioKey(Guid meditationId)
    {
        return $"audios/{meditationId}";
    }
}