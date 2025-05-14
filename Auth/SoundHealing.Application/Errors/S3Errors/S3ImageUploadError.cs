using CQRS;
using SoundHealing.Application.Interfaces;

namespace SoundHealing.Application.Errors.S3Errors;

public class S3ImageUploadError(Guid meditationId) 
    : ErrorResponse($"Не удалось загрузить изображение медитации с Id {meditationId} в S3");