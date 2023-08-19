using System.Collections;
using Registration.RequestForm;
using Registration.ResponseForm;
using Request;
using Response;
using UnityEngine.Networking;

namespace Registration
{
    public class EmailConfirmationRequest : IRequest
    {
        private EmailConfirmationForm _confirmationForm;

        public EmailConfirmationRequest(string email, string code)
        {
            _confirmationForm = new EmailConfirmationForm {email = email, code = code};
        }

        public UnityWebRequest Body { get; private set; }
        public object Result { get; private set; }

        public IEnumerator Send()
        {
            using (Body = new UnityWebRequest(API.EMAIL_VERIFICATION, RequestMethod.POST) 
            {
                uploadHandler = new UploadHandlerRaw(_confirmationForm.ToBytes()), 
                downloadHandler = new DownloadHandlerBuffer()
            })
            {
                Body.SetRequestHeader("Content-Type", "application/json");
                yield return Body.SendWebRequest();
                Result = this.GetResponse<EmailConfirmationSuccess>();
            }
        }
    }
}