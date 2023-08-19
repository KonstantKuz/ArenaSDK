using System;
using UnityEngine.Networking;

namespace Response.Fail
{
    [Serializable]
    public class UnityFail : IFailResponse
    {
        public string message;

        public static UnityFail CreateFromRequest(UnityWebRequest request)
        {
            return new() {message = request.error};
        }

        public string Message => message;
    }
}