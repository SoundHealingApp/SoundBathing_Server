namespace SoundHealing.Core;

public static class PermissionsConstants
{
    public static readonly string[] AdminPermissions =
    [
        LiveStreamsAdministration,
        MeditationsAdministration,
        QuotesAdministration
    ];
    
    public static readonly string[] UserPermissions =
    [
        GetUserInfo,
        EditUserInfo,
        GetLiveStreamsInfo,
        GetMeditationsInfo,
        GetQuotesInfo,
        AddFeedback,
        GetFeedbackInfo,
        ManageMeditationsLikes,
        ManageMeditationsRecommendations
    ];
    
    // Admin permissions
    public const string LiveStreamsAdministration = nameof(LiveStreamsAdministration);
    public const string MeditationsAdministration = nameof(MeditationsAdministration);
    public const string QuotesAdministration = nameof(QuotesAdministration);
    
    // User permissions
    public const string GetUserInfo = nameof(GetUserInfo);
    public const string EditUserInfo = nameof(EditUserInfo);
    public const string GetLiveStreamsInfo = nameof(GetLiveStreamsInfo);
    public const string GetMeditationsInfo = nameof(GetMeditationsInfo);
    public const string GetQuotesInfo = nameof(GetQuotesInfo);
    public const string AddFeedback = nameof(AddFeedback); // admin can't
    public const string GetFeedbackInfo = nameof(GetFeedbackInfo);
    public const string ManageMeditationsLikes = nameof(ManageMeditationsLikes);
    public const string ManageMeditationsRecommendations = nameof(ManageMeditationsRecommendations);
}