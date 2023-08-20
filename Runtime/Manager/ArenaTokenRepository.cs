using System;
using Authorization;
using JetBrains.Annotations;
using Response;
using UnityEngine;

namespace Manager
{
    internal static class ArenaTokenRepository
    {
        private const string DATE_KEY = "date";

        internal static void TrySaveTokens(IResponse authorizationResponse)
        {
            if(authorizationResponse is not AuthorizationSuccess success) return;
            SaveToken(TokenType.AccessToken, success.accessToken);
            SaveToken(TokenType.RefreshToken, success.refreshToken);
        }
        
        internal static bool IsValid(TokenType tokenType)
        {
            var token = LoadToken(tokenType);
            var currentLifetime = DateTime.Now - JsonUtility.FromJson<DateTime>(tokenType + DATE_KEY);
            return currentLifetime.Milliseconds < token.expiresIn;
        }

        internal static TimeSpan GetLifetimeLeft(TokenType tokenType)
        {
            var token = LoadToken(tokenType);
            var currentLifetime = DateTime.Now - JsonUtility.FromJson<DateTime>(tokenType + DATE_KEY);
            return TimeSpan.FromMilliseconds(token.expiresIn - currentLifetime.Milliseconds);
        }
    
        internal static void SaveToken(TokenType tokenType, JWTTokenResponse token)
        {
            PlayerPrefs.SetString(tokenType.ToString(), JsonUtility.ToJson(token));
            PlayerPrefs.SetString(tokenType + DATE_KEY, JsonUtility.ToJson(DateTime.Now));
        }

        internal static JWTTokenResponse LoadToken(TokenType tokenType)
        {
            return JsonUtility.FromJson<JWTTokenResponse>(tokenType.ToString());
        }
    }
}