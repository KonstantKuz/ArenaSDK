using System.Collections;
using Authorization.RequestForm;
using Authorization.ResponseForm;
using Request;
using UnityEngine.Networking;

namespace Authorization
{
    public class AuthorizationRequest : IRequest
    {
        private AuthorizationForm _authorizationForm;
        
        public UnityWebRequest Body { get; private set; }
        public object Result { get; private set; }

        public AuthorizationRequest(string login, string password)
        {
            _authorizationForm = new AuthorizationForm {login = login, password = password};
        }
        
        public IEnumerator Send()
        {
            if (!this.IsFormValid(_authorizationForm, out var fail))
            {
                Result = fail;
                yield break;
            }

            using (Body = new UnityWebRequest(API.AUTHORIZATION, RequestMethod.POST)
            {
                uploadHandler = new UploadHandlerRaw(_authorizationForm.ToBytes()),
                downloadHandler = new DownloadHandlerBuffer()
            })
            {
                Body.SetRequestHeader("Content-Type", "application/json");
                yield return Body.SendWebRequest();
                Result = this.GetResponse<AuthorizationTokens>();
            }
        }
    }
}