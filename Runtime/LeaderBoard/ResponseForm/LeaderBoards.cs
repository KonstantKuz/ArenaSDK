using System;
using System.Collections.Generic;
using Response;

namespace LeaderBoard.ResponseForm
{
    [Serializable]
    public class LeaderBoards : IResponse
    {
        public List<LeaderBoardItem> leaderboards;
        public List<LeaderBoardItem> aroundLeaderboards;
    }
}