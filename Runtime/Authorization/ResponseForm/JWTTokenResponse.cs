using System;

namespace Authorization.ResponseForm
{
    [Serializable]
    public class JWTTokenResponse
    {
        public int expiresIn;
        public string token;
    }
}