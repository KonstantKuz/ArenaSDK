using System;
using Authorization;
using Response;
using Response.Fail;
using UnityEngine;

namespace Samples.Scripts
{
    public class SampleAuthorization : MonoBehaviour
    {
        [SerializeField] private AuthorizationWindow _authorizationWindow;
        [SerializeField] private MessagePopup _messagePopup;
        
        public JWTTokenResponse AccessToken { get; private set; }
        public JWTTokenResponse RefreshToken { get; private set; }

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
            ArenaSDKManager.Instance.AuthorizeUser(username, password, AuthorizationCallback, UpdateTokenCallback);
        }

        private void AuthorizationCallback(IResponse response)
        {
            switch (response)
            {
                case AuthorizationSuccess success:
                    RefreshToken = success.refreshToken;
                    AccessToken = success.accessToken;
                    _messagePopup.Show("Authorization success!");
                    break;
                case IFailResponse fail:
                    _messagePopup.Show(fail.Message);
                    break;
            }
        }

        private void UpdateTokenCallback(JWTTokenResponse token)
        {
            AccessToken = token;
        }
    }
}