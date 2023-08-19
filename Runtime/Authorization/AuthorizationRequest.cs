using System.Collections;
using Request;
using Response.Fail;
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
            if (!_authorizationForm.IsValid(out var description))
            {
                Result = new InvalidFormFail(description);
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
                Result = this.GetResponse<AuthorizationSuccess>();
            }
        }
    }
}