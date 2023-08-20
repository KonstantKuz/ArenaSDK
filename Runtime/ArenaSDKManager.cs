using System;
using System.Collections;
using Authorization;
using LeaderBoard;
using Manager;
using Registration;
using Request;
using Response;
using Response.Fail;
using UnityEngine;
using UnityEngine.Assertions;
using Util;

public class ArenaSDKManager : MonoBehaviour
{
    [SerializeField] private string _gameAlias = "FIGHTER";

    private CompositeDisposable _disposable = new();
    
    public static ArenaSDKManager Instance { get; private set; }
    public event Action<IFailResponse> OnTokenUpdateFail;

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

    public void AuthorizeUser(string userNameOrEmail, string password, Action<IResponse> callback)
    {
        var request = new AuthorizationRequest(userNameOrEmail, password);
        Action<IResponse> onResponseReceived = response =>
        {
            ArenaTokenRepository.TrySaveTokens(response);
            ArenaTokenUpdater.TryRunAutoUpdate(response, this, it => OnTokenUpdateFail?.Invoke(it))
                .AddTo(_disposable);
            callback?.Invoke(response);
        };
        StartCoroutine(SendRequest(request, onResponseReceived));
    }

    public void LoadLeaderBoard(string leaderboardAlias, Action<IResponse> callback)
    {
        var request = new GetLeaderBoardRequest(leaderboardAlias, ArenaTokenRepository.LoadToken(TokenType.AccessToken));
        StartCoroutine(SendRequest(request, callback));
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