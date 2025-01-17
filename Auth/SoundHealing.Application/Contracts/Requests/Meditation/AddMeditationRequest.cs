using SoundHealing.Core.Enums;

namespace SoundHealing.Application.Contracts.Requests.Meditation;

public record AddMeditationRequest(
    string Title,
    string Description,
    MeditationType MeditationType,
    string TherapeuticPurpose,
    string ImageLink,
    string VideoLink,
    double Frequency);