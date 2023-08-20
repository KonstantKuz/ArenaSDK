using System;
using UnityEngine.Networking;

namespace Response.Fail
{
    [Serializable]
    public class UnityWebRequestFail : IFailResponse
    {
        public string message;

        public static UnityWebRequestFail CreateFromRequest(UnityWebRequest request)
        {
            return new() {message = request.error};
        }

        public string Message => message;
    }
}