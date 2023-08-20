using System.Collections;
using Request;
using UnityEngine.Networking;

namespace User
{
    public class GetUserInfoRequest : IRequest
    {
        private readonly string _accessToken;

        public GetUserInfoRequest(string accessToken)
        {
            _accessToken = accessToken;
        }
        
        public UnityWebRequest Body { get; private set; }
        public object Result { get; private set; }
        
        public IEnumerator Send()
        {
            using (Body = new UnityWebRequest(API.GET_PROFILE, RequestMethod.GET)
            {
                downloadHandler = new DownloadHandlerBuffer()
            })
            {
                Body.SetRequestHeader("Content-Type", "application/json");
                Body.SetRequestHeader("access-token", _accessToken);
                yield return Body.SendWebRequest();
                Result = this.GetResponse<UserInfo>();
            }
        }
    }
}