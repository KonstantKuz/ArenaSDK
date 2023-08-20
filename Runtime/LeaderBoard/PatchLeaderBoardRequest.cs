using System.Collections;
using LeaderBoard.RequestForm;
using Request;
using Response;
using UnityEngine.Networking;

namespace LeaderBoard
{
    public class PatchLeaderBoardRequest : IRequest
    {
        private string _leaderboardAlias;
        private string _serverToken;
        
        private PatchLeaderboardForm _patchLeaderboardForm;
        
        public UnityWebRequest Body { get; private set; }
        public object Result { get; private set;  }

        public PatchLeaderBoardRequest(string leaderboardAlias, string serverToken, string profileId, int value)
        {
            _leaderboardAlias = leaderboardAlias;
            _serverToken = serverToken;
            _patchLeaderboardForm = new PatchLeaderboardForm { profileId = profileId, value = value,};
        }
        
        public IEnumerator Send()
        {
            using (Body = new UnityWebRequest(API.GET_LEADERBOARD(_leaderboardAlias), RequestMethod.PATCH)
            {
                uploadHandler = new UploadHandlerRaw(_patchLeaderboardForm.ToBytes()),
                downloadHandler = new DownloadHandlerBuffer()
            })
            {
                Body.SetRequestHeader("Content-Type", "application/json");
                Body.SetRequestHeader("x-auth-server", _serverToken);
                yield return Body.SendWebRequest();
                Result = this.GetResponse<OperationSuccess>();
            }
        }
    }
}