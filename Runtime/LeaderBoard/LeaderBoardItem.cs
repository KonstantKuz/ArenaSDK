using System;

namespace LeaderBoard
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