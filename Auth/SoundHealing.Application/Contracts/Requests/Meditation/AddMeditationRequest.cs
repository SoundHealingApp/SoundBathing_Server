using Microsoft.AspNetCore.Http;
using SoundHealing.Core.Enums;

namespace SoundHealing.Application.Contracts.Requests.Meditation;

public record AddMeditationRequest(
    string Title,
    string Description,
    MeditationType MeditationType,
    string? TherapeuticPurpose, // todo: change to non-nullable
    IFormFile Image,
    IFormFile Audio,
    double? Frequency);