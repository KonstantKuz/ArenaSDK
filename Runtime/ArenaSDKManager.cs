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

    private CompositeDisposable _disposable = new CompositeDisposable();
    private ArenaUserRepository _userRepository;
    
    private static ArenaSDKManager _instance;
    public static ArenaSDKManager Instance => _instance ??= Init();

    [CanBeNull]
    public static UserInfo UserInfo => Instance._userRepository.Info;
    public static AuthorizationTokens Tokens => ArenaTokenRepository.LoadTokens();
    
    public event Action<IFailResponse> OnAccessTokenUpdateFailed;
    
    /// <summary>
    /// Possible response : UserInfoLoadFail, UnityWebRequestFail, ServerFail
    /// </summary>
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

    /// <summary>
    /// User registration
    /// </summary>
    /// <param name="username">Min characters : 3 Max characters : 20 Valid characters : a-z, A-Z, 0-9</param>
    /// <param name="password">Min characters : 8</param>
    /// <param name="callback">Possible responses : UserInfo (success), InvalidFormFail (invalid parameters provided), UnityWebRequestFail, ServerFail</param>
    public void RegisterUser(string email, string username, string password, Action<IResponse> callback)
    {
        var request = new UserRegistrationRequest(email, username, password);
        StartCoroutine(SendRequest(request, callback));
    }

    /// <summary>
    /// Send confirmation code to user's email
    /// </summary>
    /// <param name="callback">Possible responses : OperationSuccess (success), UnityWebRequestFail, ServerFail</param>
    public void SendConfirmationCode(string email, Action<IResponse> callback)
    {
        var request = new SendCodeRequest(email);
        StartCoroutine(SendRequest(request, callback));
    }

    /// <summary>
    /// Try to confirm user's code
    /// </summary>
    /// <param name="code"></param>
    /// <param name="callback">Possible responses : OperationSuccess (success), UnityWebRequestFail, ServerFail</param>
    public void ConfirmEmail(string email, string code, Action<IResponse> callback)
    {
        var request = new EmailConfirmationRequest(email, code);
        StartCoroutine(SendRequest(request, callback));
    }

    /// <summary>
    /// User authorization
    /// </summary>
    /// <param name="userNameOrEmail">Min characters : 1</param>
    /// <param name="password">Min characters : 8 Max characters : 100</param>
    /// <param name="callback">Possible responses : AuthorizationTokens (success), InvalidFormFail (invalid parameters provided), UnityWebRequestFail, ServerFail</param>
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

    /// <summary>
    /// Load UserInfo
    /// </summary>
    /// <param name="callback">Possible responses : UserInfo (success), UnityWebRequestFail, ServerFail</param>
    public void LoadUserInfo(Action<IResponse> callback)
    {
        var token = ArenaTokenRepository.LoadToken(TokenType.AccessToken).token;
        var request = new GetUserInfoRequest(token);
        StartCoroutine(SendRequest(request, callback));
    }

    /// <summary>
    /// Load leaderboards
    /// </summary>
    /// <param name="leaderboardAlias"></param>
    /// <param name="limit">The maximum number of items to be returned in a single response. For instance, if the limit is set to 10, then the API will return the first 10 items that match the query.</param>
    /// <param name="offset">Indicates where to start fetching the data. For example, if the offset is set to 20, then the API will skip the first 20 items that match the query and start fetching from the 21st item.</param>
    /// <param name="callback">Possible responses : LeaderBoards (success), InvalidFormFail (invalid parameters provided), UnityWebRequestFail, ServerFail</param>
    public void LoadLeaderBoard(string leaderboardAlias, Action<IResponse> callback, int limit = 10, int offset = 0)
    {
        var token = ArenaTokenRepository.LoadToken(TokenType.AccessToken).token;
        var request = new GetLeaderBoardRequest(leaderboardAlias, token, limit, offset);
        StartCoroutine(SendRequest(request, callback));
    }

    /// <summary>
    /// Send user's new score
    /// </summary>
    /// <param name="callback">Possible responses : LeaderBoards (success), UserInfoLoadFail, UnityWebRequestFail, ServerFail</param>
    public void UpdateUserStatistics(string leaderboardAlias, int value, Action<IResponse> callback)
    {
        if (UserInfo == null)
        {
            callback?.Invoke(new UserInfoLoadFail());
            return;
        }

        var request = new PatchLeaderBoardRequest(leaderboardAlias, _serverToken, UserInfo.id, value);
        StartCoroutine(SendRequest(request, callback));
    }

    /// <summary>
    /// Manually request new access token
    /// </summary>
    /// <param name="callback">Possible responses : RefreshTokenSuccess (success), UnityWebRequestFail, ServerFail</param>
    public void UpdateAccessToken(Action<IResponse> callback)
    {
        var refreshToken = ArenaTokenRepository.LoadToken(TokenType.RefreshToken);
        var request = new RefreshTokenRequest(refreshToken.token);
        Action<IResponse> onResponseReceived = response =>
        {
            callback?.Invoke(response);
            if(response is not RefreshTokenSuccess success) return;
            ArenaTokenRepository.SaveToken(TokenType.AccessToken, success.accessToken);
        };
        StartCoroutine(SendRequest(request, onResponseReceived));
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