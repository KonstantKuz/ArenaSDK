using System;
using LeaderBoard.RequestForm;
using Manager;
using Request;
using UnityEngine.Assertions;
using Util;

namespace LeaderBoard
{
    public class GetLeaderBoardRequestBuilder
    {
        private GetLeaderBoardRequest _request;

        public GetLeaderBoardRequestBuilder(string alias, string profileId)
        {
            _request = new GetLeaderBoardRequest(alias);
            _request._profileId = profileId;
        }

        public GetLeaderBoardRequestBuilder SetAccessToken(TokenType type, string token)
        {
            if (type == TokenType.RefreshToken) throw new ArgumentException("Get leader board request can only have access or server token header type.");
            _request._tokenType = type;
            _request._token = token;
            return this;
        }

        public GetLeaderBoardRequestBuilder SetRequestTarget(RequestTarget requestTarget)
        {
            _request._requestTarget = requestTarget;
            return this;
        }

        public GetLeaderBoardRequestBuilder SetVersion(string version)
        {
            _request._leaderboardVersion = version;
            return this;
        }
        
        public GetLeaderBoardRequest Build(int limit = 10, int offset = 0, int aroundPlayerLimit = 10, bool isAroundPlayer = true)
        {
            Assert.IsFalse(_request._token.IsNullOrEmpty(), "Must set request token.");
            BuildUrl();
            SetRequestHeader();
            BuildForm(limit, offset, aroundPlayerLimit, isAroundPlayer);
            return _request;
        }

        private void BuildUrl()
        {
            if (_request._leaderboardVersion.IsNullOrEmpty())
            {
                _request._requestUrl = API.GET_LEADERBOARD(_request._leaderboardAlias);
                return;
            }
            Assert.IsFalse(_request._profileId.IsNullOrEmpty(), "Must set profileId for request with version.");
            _request._requestUrl =
                API.GET_LEADERBOARD_VERSION_FILTER(_request._leaderboardAlias, _request._leaderboardVersion, _request._requestTarget);
        }

        private void SetRequestHeader()
        {
            _request._header = _request._tokenType switch
            {
                TokenType.AccessToken => TokenHeader.ACCESS_TOKEN,
                TokenType.ServerToken => TokenHeader.SERVER_TOKEN,
                TokenType.RefreshToken => throw new ArgumentException("Get leader board request can only have access or server token header type."),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void BuildForm(int limit = 10, int offset = 0, int aroundPlayerLimit = 10, bool isAroundPlayer = true)
        {
            _request._getLeaderboardForm = new GetLeaderboardForm
            {
                limit = limit.ToString(),
                offset = offset.ToString(), 
                aroundPlayerLimit = aroundPlayerLimit.ToString(),
                isAroundPlayer = isAroundPlayer.ToString().ToLower(),
                profileId = _request._profileId,
            };
        }
    }
}