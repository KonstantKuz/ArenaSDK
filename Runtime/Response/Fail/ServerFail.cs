using System;
using System.Text;
using Util;

namespace Response.Fail
{
    [Serializable]
    public class ServerFail : IFailResponse
    {
        public string id;
        public string code;
        public string type;
        public string message;
        public string[] @params;
        
        public string Message => CreateMessage();

        private string CreateMessage()
        {
            var builder = new StringBuilder($"Server failed : {message}");
            if (@params == null) return builder.ToString();
            foreach (var value in @params) builder.Append($"\n {value}");
            return builder.ToString();
        }
        
        public bool HasValue()
        {
            return !type.IsNullOrEmpty() && type.Contains("error");
        }
    }
}