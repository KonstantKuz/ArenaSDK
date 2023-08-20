using System;

namespace Authorization
{
    [Serializable]
    public class JWTTokenResponse
    {
        public int expiresIn;
        public string token;
    }
}