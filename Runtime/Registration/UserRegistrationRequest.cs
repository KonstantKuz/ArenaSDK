using System.Collections;
using Registration.RequestForm;
using Registration.ResponseForm;
using Request;
using Response.Fail;
using UnityEngine.Networking;

namespace Registration
{
    public class UserRegistrationRequest : IRequest
    {
        private readonly UserRegistrationForm _registrationForm;

        public UserRegistrationRequest(string email, string username, string password)
        {
            _registrationForm = new UserRegistrationForm {email = email, username = username, password = password};
        }

        public UnityWebRequest Body { get; private set; }
        public object Result { get; private set; }

        public IEnumerator Send()
        {
            if (!_registrationForm.IsValid(out var description))
            {
                Result = new InvalidFormFail(description);
                yield break;
            }

            using (Body = new UnityWebRequest(API.USER_REGISTRATION, RequestMethod.POST)
            {
                uploadHandler = new UploadHandlerRaw(_registrationForm.ToBytes()),
                downloadHandler = new DownloadHandlerBuffer()
            })
            {
                Body.SetRequestHeader("Content-Type", "application/json");
                yield return Body.SendWebRequest();
                Result = this.GetResponse<UserRegistrationSuccess>();
            }
        }
    }
}
