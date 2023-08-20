using System;
using Response;

namespace Authorization.ResponseForm
{
    [Serializable]
    public class AuthorizationTokens : IResponse
    {
        public JWTTokenResponse accessToken;
        public JWTTokenResponse refreshToken;
    }
}