using System;

namespace LeaderBoard.RequestForm
{
    [Serializable]
    public class PatchLeaderboardForm
    {
        public string profileId;
        public int value;
    }
}