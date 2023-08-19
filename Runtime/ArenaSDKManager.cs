using System;
using System.Collections;
using Authorization;
using Registration;
using Request;
using Response;
using Response.Fail;
using UnityEngine;
using UnityEngine.Assertions;

public class ArenaSDKManager : MonoBehaviour
{
    [SerializeField] private string _gameAlias = "FIGHTER";
    
    public static ArenaSDKManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        GameData.ALIAS = _gameAlias;
        Assert.IsTrue(_gameAlias != null && !_gameAlias.Equals(string.Empty), "Game alias is empty.");
    }

    public void RegisterUser(string email, string username, string password, Action<IResponse> callback)
    {
        var request = new UserRegistrationRequest(email, username, password);
        StartCoroutine(SendRequest(request, callback));
    }

    public void SendConfirmationCode(string email, Action<IResponse> callback)
    {
        var request = new SendCodeRequest(email);
        StartCoroutine(SendRequest(request, callback));
    }

    public void ConfirmEmail(string email, string code, Action<IResponse> callback)
    {
        var request = new EmailConfirmationRequest(email, code);
        StartCoroutine(SendRequest(request, callback));
    }

    public void AuthorizeUser(string userNameOrEmail, string password, Action<IResponse> callback, Action<JWTTokenResponse> onAccessTokenUpdate)
    {
        var request = new AuthorizationRequest(userNameOrEmail, password);
        Action<IResponse> onResponseReceived = response =>
        {
            callback?.Invoke(response);
            AuthorizationCallback(response, onAccessTokenUpdate);
        };
        StartCoroutine(SendRequest(request, onResponseReceived));
    }

    private void AuthorizationCallback(IResponse response, Action<JWTTokenResponse> updateCallback)
    {
        if(!(response is AuthorizationSuccess success)) return;
        StartCoroutine(AutoUpdateAccessToken(success.refreshToken, success.accessToken, updateCallback));
    }

    private IEnumerator AutoUpdateAccessToken(JWTTokenResponse refreshToken, JWTTokenResponse accessToken, Action<JWTTokenResponse> updateCallback)
    {
        var seconds = (float) TimeSpan.FromMilliseconds(accessToken.expiresIn).TotalSeconds;
        yield return new WaitForSeconds(seconds);
        
        var request = new RefreshTokenRequest(refreshToken.token);
        yield return request.Send();
        switch (request.Result)
        {
            case RefreshTokenSuccess success:
                updateCallback?.Invoke(success.accessToken);
                StartCoroutine(AutoUpdateAccessToken(refreshToken, success.accessToken, updateCallback));
                break;
            case IFailResponse fail: 
                Debug.LogError($"Refresh token update failed : {fail.Message}");
                break;
        }
    }

    private IEnumerator SendRequest(IRequest request, Action<IResponse> callback)
    {
        yield return request.Send();
        callback?.Invoke((IResponse) request.Result);
    }
}