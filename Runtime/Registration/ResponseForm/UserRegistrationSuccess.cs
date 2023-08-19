using System;
using Response;
using UnityEngine;

namespace Registration.ResponseForm
{
    [Serializable]
    public class UserRegistrationSuccess : IResponse
    {
        public string id;
        public string username;
        public Playfab playfab;
        public string avatar;
        public string email;
        
        [Serializable]
        public class Playfab
        {
            public string playfabId;
            public string token;
            public int tokenExpired;
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}