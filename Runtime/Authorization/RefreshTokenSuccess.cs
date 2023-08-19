using System;
using Response;

namespace Authorization
{
    [Serializable]
    public class RefreshTokenSuccess : IResponse
    {
        public JWTTokenResponse accessToken;
    }
}