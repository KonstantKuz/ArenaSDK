using System.Collections;
using LeaderBoard.RequestForm;
using LeaderBoard.ResponseForm;
using Manager;
using Request;
using UnityEngine.Networking;

namespace LeaderBoard
{
    public class GetLeaderBoardRequest : IRequest
    {
        public readonly string _leaderboardAlias;

        public string _profileId;
        public string _leaderboardVersion;
        public string _requestUrl;
        public RequestTarget _requestTarget;
        public TokenType _tokenType;
        public string _token;
        public string _header;
        public GetLeaderboardForm _getLeaderboardForm;
        
        public UnityWebRequest Body { get; private set; }
        public object Result { get; private set;  }
        
        public GetLeaderBoardRequest(string leaderboardAlias)
        {
            _leaderboardAlias = leaderboardAlias;
        }

        public IEnumerator Send()
        {
            if (!this.IsFormValid(_getLeaderboardForm, out var fail))
            {
                Result = fail;
                yield break;
            }

            using (Body = new UnityWebRequest(_requestUrl, RequestMethod.GET)
            {
                uploadHandler = new UploadHandlerRaw(_getLeaderboardForm.ToBytes()),
                downloadHandler = new DownloadHandlerBuffer()
            })
            {
                Body.SetRequestHeader("Content-Type", "application/json");
                Body.SetRequestHeader(_header, _token);
                yield return Body.SendWebRequest();
                Result = this.GetResponse<LeaderBoards>();
            }
        }
    }
}