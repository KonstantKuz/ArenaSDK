using Authorization;
using Response;
using Response.Fail;
using UnityEngine;

namespace Samples.Scripts
{
    public class AuthorizationSample : MonoBehaviour
    {
        [SerializeField] private AuthorizationWindow _authorizationWindow;
        [SerializeField] private MessagePopup _messagePopup;

        private void Awake()
        {
            _authorizationWindow.Confirm.onClick.AddListener(OnAuthorizationConfirm);
        }

        private void OnAuthorizationConfirm()
        {
            Authorize(_authorizationWindow.Login.text, _authorizationWindow.Password.text);
        }

        public void ProceedToLogin(string username, string password)
        {
            Authorize(username, password);
        }

        public void Authorize(string username, string password)
        {
            ArenaSDKManager.Instance.AuthorizeUser(username, password, AuthorizationCallback);
        }

        private void AuthorizationCallback(IResponse response)
        {
            switch (response)
            {
                case AuthorizationSuccess success:
                    _messagePopup.Show("Authorization success!");
                    break;
                case IFailResponse fail:
                    _messagePopup.Show(fail.Message);
                    break;
            }
        }
    }
}