using System;
using Response;

namespace Authorization
{
    [Serializable]
    public class AuthorizationSuccess : IResponse
    {
        public JWTTokenResponse accessToken;
        public JWTTokenResponse refreshToken;
    }
}