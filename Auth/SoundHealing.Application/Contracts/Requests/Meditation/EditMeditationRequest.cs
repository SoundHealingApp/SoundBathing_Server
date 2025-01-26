using Microsoft.AspNetCore.Http;
using SoundHealing.Core.Enums;

namespace SoundHealing.Application.Contracts.Requests.Meditation;

public record EditMeditationRequest(
    string? Title,
    string? Description,
    MeditationType? MeditationType,
    string? TherapeuticPurpose,
    IFormFile? Image,
    IFormFile? Audio,
    double? Frequency);