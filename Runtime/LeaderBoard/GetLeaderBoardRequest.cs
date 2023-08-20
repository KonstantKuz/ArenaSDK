using System.Collections;
using LeaderBoard.RequestForm;
using LeaderBoard.ResponseForm;
using Request;
using UnityEngine.Networking;

namespace LeaderBoard
{
    public class GetLeaderBoardRequest : IRequest
    {
        private string _leaderboardAlias;
        private string _accessToken;
        
        private GetLeaderboardForm _getLeaderboardForm;
        
        public UnityWebRequest Body { get; private set; }
        public object Result { get; private set;  }

        public GetLeaderBoardRequest(string leaderboardAlias, string accessToken,
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
                downloadHandler = new DownloadHandlerBuffer()
            })
            {
                Body.SetRequestHeader("Content-Type", "application/json");
                Body.SetRequestHeader("access-token", _accessToken);
                yield return Body.SendWebRequest();
                Result = this.GetResponse<LeaderBoards>();
            }
        }
    }
}