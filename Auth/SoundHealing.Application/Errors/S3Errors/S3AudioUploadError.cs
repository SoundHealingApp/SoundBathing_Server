using CQRS;

namespace SoundHealing.Application.Errors.S3Errors;

public class S3AudioUploadError(Guid meditationId) 
    : ErrorResponse($"Не удалось загрузить аудио медитации с Id {meditationId} в S3");