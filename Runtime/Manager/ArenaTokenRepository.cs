using System;
using Authorization.ResponseForm;
using UnityEngine;

namespace Manager
{
    internal static class ArenaTokenRepository
    {
        private const string SAVE_DATE_KEY = "savedate";

        internal static void SaveTokens(AuthorizationTokens tokens)
        {
            SaveToken(TokenType.AccessToken, tokens.accessToken);
            SaveToken(TokenType.RefreshToken, tokens.refreshToken);
        }

        internal static AuthorizationTokens LoadTokens()
        {
            return new AuthorizationTokens
            {
                accessToken = LoadToken(TokenType.AccessToken),
                refreshToken = LoadToken(TokenType.RefreshToken),
            };
        }
        
        internal static bool IsValid(TokenType tokenType)
        {
            var token = LoadToken(tokenType);
            var currentLifetime = DateTime.Now - JsonUtility.FromJson<DateTime>(tokenType + SAVE_DATE_KEY);
            return currentLifetime.Milliseconds < token.expiresIn;
        }

        internal static TimeSpan GetLifetimeLeft(TokenType tokenType)
        {
            var token = LoadToken(tokenType);
            var saveTime = JsonUtility.FromJson<DateTime>(PlayerPrefs.GetString(tokenType + SAVE_DATE_KEY));
            var currentLifetime = DateTime.Now - saveTime;
            return TimeSpan.FromMilliseconds(token.expiresIn - currentLifetime.Milliseconds);
        }
    
        internal static void SaveToken(TokenType tokenType, JWTTokenResponse token)
        {
            PlayerPrefs.SetString(tokenType.ToString(), JsonUtility.ToJson(token));
            PlayerPrefs.SetString(tokenType + SAVE_DATE_KEY, JsonUtility.ToJson(DateTime.Now));
        }

        internal static JWTTokenResponse LoadToken(TokenType tokenType)
        {
            var saved = PlayerPrefs.GetString(tokenType.ToString());
            return JsonUtility.FromJson<JWTTokenResponse>(saved);
        }
    }
}