using System;
using Response;

namespace User
{
    [Serializable]
    public class UserInfo : IResponse
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
    }
}