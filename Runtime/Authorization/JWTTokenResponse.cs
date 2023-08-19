using System;
using UnityEngine;

namespace Authorization
{
    [Serializable]
    public class JWTTokenResponse
    {
        public int expiresIn;
        public string token;

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}