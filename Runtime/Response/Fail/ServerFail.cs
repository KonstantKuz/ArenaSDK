using System;

namespace Response.Fail
{
    [Serializable]
    public class ServerFail : IFailResponse
    {
        public string id;
        public string code;
        public string type;
        public string message;
        
        public string Message => message;
    }
}