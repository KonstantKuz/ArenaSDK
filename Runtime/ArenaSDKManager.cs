using System;
using System.Collections;
using Authorization;
using Authorization.ResponseForm;
using JetBrains.Annotations;
using LeaderBoard;
using Manager;
using Registration;
using Request;
using Response;
using Response.Fail;
using UnityEngine;
using UnityEngine.Assertions;
using User;
using Util;

public class ArenaSDKManager : MonoBehaviour
{
    [SerializeField] private string _gameAlias = "FIGHTER";
    [SerializeField] private string _serverToken = "";
    [SerializeField] private int _maxUserLoadAttemptCount = 10;

    private CompositeDisposable _disposable = new();
    private ArenaUserRepository _userRepository;
    
    private static ArenaSDKManager _instance;
    public static ArenaSDKManager Instance => _instance ??= Init();

    [CanBeNull]
    public UserInfo UserInfo => _userRepository.Info;
    
    public event Action<IFailResponse> OnAccessTokenUpdateFailed;
    public event Action<IFailResponse> OnUserInfoLoadFailed;

    private static ArenaSDKManager Init()
    {
        var managers = FindObjectsOfType<ArenaSDKManager>();
        if (managers.Length == 0) throw new NullReferenceException("ArenaSDKManager not found.");
        if (managers.Length > 1) throw new Exception("There are more than one instances of ArenaSDKManager.");
        var instance = managers[0];
        Assert.IsTrue(instance._gameAlias != null && !instance._gameAlias.Equals(string.Empty), "Game alias is empty.");
        GameData.ALIAS = instance._gameAlias;
        DontDestroyOnLoad(instance);
        return instance;
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

    public void AuthorizeUser(string userNameOrEmail, string password, Action<IResponse> callback)
    {
        var request = new AuthorizationRequest(userNameOrEmail, password);
        Action<IResponse> onResponseReceived = response =>
        {
            OnAuthorizationResponse(response);
            callback?.Invoke(response);
        };
        StartCoroutine(SendRequest(request, onResponseReceived));
    }

    public void LoadUserInfo(Action<IResponse> callback)
    {
        var token = ArenaTokenRepository.LoadToken(TokenType.AccessToken).token;
        var request = new GetUserInfoRequest(token);
        StartCoroutine(SendRequest(request, callback));
    }

    public void LoadLeaderBoard(string leaderboardAlias, Action<IResponse> callback)
    {
        var token = ArenaTokenRepository.LoadToken(TokenType.AccessToken).token;
        var request = new GetLeaderBoardRequest(leaderboardAlias, token);
        StartCoroutine(SendRequest(request, callback));
    }

    public void UpdateUserStatistics(string leaderboardAlias, int value, Action<IResponse> callback)
    {
        if (_userRepository.Info == null)
        {
            callback?.Invoke(new UserInfoLoadFail());
            return;
        }

        var request = new PatchLeaderBoardRequest(leaderboardAlias, _serverToken, _userRepository.Info.id, value);
        StartCoroutine(SendRequest(request, callback));
    }

    private void OnAuthorizationResponse(IResponse response)
    {
        if(response is IFailResponse) return;
        var success = response as AuthorizationTokens;
        ArenaTokenRepository.SaveTokens(success);
        ArenaTokenUpdater.RunAutoUpdate(this, OnAccessTokenUpdateFailed)
            .AddTo(_disposable);
        _userRepository = new ArenaUserRepository(this, _maxUserLoadAttemptCount, OnUserInfoLoadFailed);
        _userRepository.AddTo(_disposable);
    }

    private IEnumerator SendRequest(IRequest request, Action<IResponse> callback)
    {
        yield return request.Send();
        callback?.Invoke((IResponse) request.Result);
    }

    public void OnDestroy()
    {
        _disposable?.Dispose();
    }
}