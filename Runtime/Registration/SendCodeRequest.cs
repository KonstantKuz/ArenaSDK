using System.Collections;
using Registration.RequestForm;
using Registration.ResponseForm;
using Request;
using UnityEngine.Networking;

namespace Registration
{
    public class SendCodeRequest : IRequest
    {
        private readonly SendCodeForm _sendCodeForm;
        
        public UnityWebRequest Body { get; private set; }
        public object Result { get; private set; }

        public SendCodeRequest(string email)
        {
            _sendCodeForm = new SendCodeForm {email = email};
        }
        
        public IEnumerator Send()
        {
            using (Body = new UnityWebRequest(API.SEND_CONFIRMATION_CODE, RequestMethod.POST)
            {
                uploadHandler = new UploadHandlerRaw(_sendCodeForm.ToBytes()),
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