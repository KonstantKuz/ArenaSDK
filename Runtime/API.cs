public class API
{
    public static readonly string USER_REGISTRATION = $"https://api.arenavs.com/api/v2/gamedev/client/{GameData.ALIAS}/user-registration";
    public static readonly string EMAIL_VERIFICATION = $"https://api.arenavs.com/api/v2/gamedev/client/{GameData.ALIAS}/user-registration/confirmation-email";
    public static readonly string SEND_CONFIRMATION_CODE = $"https://api.arenavs.com/api/v2/gamedev/client/{GameData.ALIAS}/user-registration/send-code-again";
    public static readonly string AUTHORIZATION = "https://api.arenavs.com/api/v2/gamedev/client/auth/sign-in";
    public static readonly string REFRESH = "https://api.arenavs.com/api/v2/gamedev/client/auth/refresh-token";
    public static readonly string GET_PROFILE = "https://api.arenavs.com/api/v2/gamedev/client/my-profile";
    public static string GET_LEADERBOARD(string leaderboardAlias) 
        => $"https://api.arenavs.com/api/v2/gamedev/client/{GameData.ALIAS}/leaderboard/{leaderboardAlias}";
    public static string PATCH_LEADERBOARD(string leaderboardAlias) =>
        $"https://api.arenavs.com/api/v2/gamedev/server/{GameData.ALIAS}/leaderboard/{leaderboardAlias}/score";
}