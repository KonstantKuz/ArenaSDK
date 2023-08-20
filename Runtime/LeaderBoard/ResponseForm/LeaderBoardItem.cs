using System;

namespace LeaderBoard.ResponseForm
{
    [Serializable]
    public class LeaderBoardItem
    {
        public int createdAt;
        public int position;
        public string profileId;
        public int score;
        public string username;
    }
}