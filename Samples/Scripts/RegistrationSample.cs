using Response;
using Response.Fail;
using UnityEngine;
using User;

namespace Samples.Scripts
{
    public class RegistrationSample : MonoBehaviour
    {
        [SerializeField] private RegistrationWindow _registrationWindow;
        [SerializeField] private ConfirmationWindow _emailConfirmationWindow;
        [SerializeField] private MessagePopup _messagePopup;
        [SerializeField] private AuthorizationSample _authorization;
        
        public void Awake()
        {
            _registrationWindow.Confirm.onClick.AddListener(RegisterUser);
        }

        private void RegisterUser()
        {
            ArenaSDKManager.Instance.RegisterUser(_registrationWindow.Email.text, 
                _registrationWindow.UserName.text, 
                _registrationWindow.Password.text,
                RegistrationCallback);
        }

        private void RegistrationCallback(IResponse response)
        {
            switch (response)
            {
                case UserInfo success:
                    ProceedToConfirmation();
                    break;
                case IFailResponse fail:
                    _messagePopup.Show(fail.Message);
                    break;
            }
        }

        private void ProceedToConfirmation()
        {
            _registrationWindow.gameObject.SetActive(false);
            _emailConfirmationWindow.gameObject.SetActive(true);
            _emailConfirmationWindow.SendCode.onClick.AddListener(SendCode);
            _emailConfirmationWindow.Confirm.onClick.AddListener(ConfirmEmail);
        }

        private void SendCode()
        {
            ArenaSDKManager.Instance.SendConfirmationCode(_registrationWindow.Email.text, SendCodeCallback);
        }

        private void SendCodeCallback(IResponse response)
        {
            if (response is ServerFail fail)
            {
                _messagePopup.Show(fail.message);
            }
        }

        private void ConfirmEmail()
        {
            ArenaSDKManager.Instance.ConfirmEmail(_registrationWindow.Email.text, _emailConfirmationWindow.Code.text, ConfirmationCallback);
        }

        private void ConfirmationCallback(IResponse response)
        {
            switch (response)
            {
                case OperationSuccess success:
                    _authorization.ProceedToLogin(_registrationWindow.UserName.text, _registrationWindow.Password.text);
                    gameObject.SetActive(false);
                    break;
                case IFailResponse fail:
                    _messagePopup.Show(fail.Message);
                    break;
            }
        }
    }
}
