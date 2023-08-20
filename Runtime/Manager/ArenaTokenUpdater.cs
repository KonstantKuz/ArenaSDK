using System;
using System.Collections;
using Authorization;
using Response;
using Response.Fail;
using UnityEngine;

namespace Manager
{
    internal class ArenaTokenUpdater : IDisposable
    {
        private readonly MonoBehaviour _runner;
        private Coroutine _coroutine;
        private Action<IFailResponse> _failCallback;

        private ArenaTokenUpdater(MonoBehaviour runner, Action<IFailResponse> failCallback)
        {
            _runner = runner;
            _failCallback = failCallback;
        }

        public static ArenaTokenUpdater TryRunAutoUpdate(IResponse authorizationResponse, MonoBehaviour runner, Action<IFailResponse> failCallback)
        {
            return new ArenaTokenUpdater(runner, failCallback)
                .RunIfAuthorizationSuccess(authorizationResponse);
        }

        private ArenaTokenUpdater RunIfAuthorizationSuccess(IResponse response)
        {
            if (response is AuthorizationSuccess success)
            {
                _coroutine = _runner.StartCoroutine(AutoUpdateAccessToken());
            }
            return this;
        }

        private IEnumerator AutoUpdateAccessToken()
        {
            var seconds = (int) ArenaTokenRepository.GetLifetimeLeft(TokenType.AccessToken).TotalSeconds;
            if (seconds > 0)
            {
                yield return new WaitForSeconds(seconds);
            }

            var refreshToken = ArenaTokenRepository.LoadToken(TokenType.RefreshToken);
            var request = new RefreshTokenRequest(refreshToken.token);
            yield return request.Send();
            switch (request.Result)
            {
                case RefreshTokenSuccess success:
                    _runner.StartCoroutine(AutoUpdateAccessToken());
                    break;
                case IFailResponse fail:
                    Debug.LogWarning($"Update access token failed : {fail.Message}");
                    _failCallback?.Invoke(fail);
                    break;
            }
        }

        public void Dispose()
        {
            if (_coroutine == null) return;
            _runner.StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}