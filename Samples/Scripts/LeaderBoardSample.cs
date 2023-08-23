using System.Collections.Generic;
using System.Linq;
using LeaderBoard.ResponseForm;
using Request;
using Response;
using Response.Fail;
using UnityEngine;

namespace Samples.Scripts
{
    public class LeaderBoardSample : MonoBehaviour
    {
        [SerializeField] private string _leaderboardAlias = "test-task";
        [SerializeField] private string _leaderboardVersion = "1";
        [SerializeField] private LeaderBoardWindow _leaderBoardWindow;
        [SerializeField] private MessagePopup _messagePopup;

        private LeaderBoardItem _playerScore = new LeaderBoardItem();
        
        private int _value;
        
        private void Awake()
        {
            _leaderBoardWindow.SetCallbacks(LoadLeaderBoard, AddScore);
        }

        private void LoadLeaderBoard()
        {
            ArenaSDKManager.Instance.LoadLeaderBoard(_leaderboardAlias, LoadCallback, _leaderboardVersion, RequestTarget.server);
        }

        private void LoadCallback(IResponse response)
        {
            switch (response)
            {
                case LeaderBoards success:
                    _leaderBoardWindow.UpdateView(success.leaderboards);
                    SavePlayerScore(success.leaderboards);
                    break;
                case IFailResponse fail:
                    _messagePopup.Show(fail.Message);
                    break;
            }
        }

        private void SavePlayerScore(List<LeaderBoardItem> items)
        {
            var playerInfo = ArenaSDKManager.UserInfo;
            _playerScore = items.Count == 0 ? new LeaderBoardItem() : items.First(it => it.profileId == playerInfo.id);
        }

        private void AddScore()
        {
            _playerScore.score++;
            ArenaSDKManager.Instance.UpdateUserStatistics(_leaderboardAlias, _playerScore.score, AddScoreCallback);
        }

        private void AddScoreCallback(IResponse response)
        {
            switch (response)
            {
                case OperationSuccess success:
                    LoadLeaderBoard();
                    break;
                case IFailResponse fail:
                    _messagePopup.Show(fail.Message);
                    break;
            }
        }
    }
}