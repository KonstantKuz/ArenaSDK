using System;
using Response;

namespace Authorization.ResponseForm
{
    [Serializable]
    public class RefreshTokenSuccess : IResponse
    {
        public JWTTokenResponse accessToken;
    }
}