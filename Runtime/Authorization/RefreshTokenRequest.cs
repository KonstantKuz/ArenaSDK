using System.Collections;
using Request;
using UnityEngine.Networking;

namespace Authorization
{
    public class RefreshTokenRequest : IRequest
    {
        private RefreshTokenForm _refreshTokenForm;
        
        public UnityWebRequest Body { get; private set; }
        public object Result { get; private set; }

        public RefreshTokenRequest(string refreshToken)
        {
            _refreshTokenForm = new RefreshTokenForm {refreshToken = refreshToken};
        }
        
        public IEnumerator Send()
        {
            using (Body = new UnityWebRequest(API.REFRESH, RequestMethod.POST)
            {
                uploadHandler = new UploadHandlerRaw(_refreshTokenForm.ToBytes()),
                downloadHandler = new DownloadHandlerBuffer()
            })
            {
                Body.SetRequestHeader("Content-Type", "application/json");
                Body.SetRequestHeader("refresh-token", _refreshTokenForm.refreshToken);
                yield return Body.SendWebRequest();
                Result = this.GetResponse<RefreshTokenSuccess>();
            }
        }
    }
}