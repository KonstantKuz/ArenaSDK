using System.Collections;
using Authorization;
using Request;
using UnityEngine.Networking;

namespace LeaderBoard
{
    public class GetLeaderBoardRequest : IRequest
    {
        private string _leaderboardAlias;
        private JWTTokenResponse _accessToken;
        
        private GetLeaderboardForm _getLeaderboardForm;
        
        public UnityWebRequest Body { get; private set; }
        public object Result { get; private set;  }

        public GetLeaderBoardRequest(string leaderboardAlias, JWTTokenResponse accessToken,
            int limit = 10, int offset = 0,
            int aroundPlayerLimit = 10, bool isAroundPlayer = true)
        {
            _leaderboardAlias = leaderboardAlias;
            _accessToken = accessToken;
            _getLeaderboardForm = new GetLeaderboardForm
            {
                limit = limit.ToString(),
                offset = offset.ToString(), 
                aroundPlayerLimit = aroundPlayerLimit.ToString(),
                isAroundPlayer = isAroundPlayer.ToString(),
            };
        }
        
        public IEnumerator Send()
        {
            if (!this.IsFormValid(_getLeaderboardForm, out var fail))
            {
                Result = fail;
                yield break;
            }

            using (Body = new UnityWebRequest(API.GET_LEADERBOARD(_leaderboardAlias), RequestMethod.GET)
            {
                uploadHandler = new UploadHandlerRaw(_getLeaderboardForm.ToBytes()),
                downloadHandler = new DownloadHandlerBuffer()
            })
            {
                Body.SetRequestHeader("Content-Type", "application/json");
                Body.SetRequestHeader("access-token", _accessToken.ToString());
                yield return Body.SendWebRequest();
                Result = this.GetResponse<GetLeaderBoardSuccess>();
            }
        }
    }
}