using System.Collections;
using Request;
using UnityEngine.Networking;

namespace Authorization
{
    public class RefreshTokenRequest : IRequest
    {
        private string _refreshToken;
        
        public UnityWebRequest Body { get; private set; }
        public object Result { get; private set; }

        public RefreshTokenRequest(string refreshToken)
        {
            _refreshToken = refreshToken;
        }
        
        public IEnumerator Send()
        {
            using (Body = new UnityWebRequest(API.REFRESH, RequestMethod.POST)
            {
                downloadHandler = new DownloadHandlerBuffer()
            })
            {
                Body.SetRequestHeader("Content-Type", "application/json");
                Body.SetRequestHeader("refresh-token", _refreshToken);
                yield return Body.SendWebRequest();
                Result = this.GetResponse<RefreshTokenSuccess>();
            }
        }
    }
}