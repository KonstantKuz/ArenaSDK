using System;
using System.Collections.Generic;
using Response;

namespace LeaderBoard
{
    [Serializable]
    public class GetLeaderBoardSuccess : IResponse
    {
        public List<LeaderBoardItem> leaderboards;
        public List<LeaderBoardItem> aroundLeaderboards;
    }
}