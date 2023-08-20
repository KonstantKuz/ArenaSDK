using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LeaderBoard.ResponseForm;
using Response;
using Response.Fail;
using UnityEngine;
using UnityEngine.UI;

namespace Samples.Scripts
{
    public class LeaderBoardSample : MonoBehaviour
    {
        [SerializeField] private string _leaderboardAlias = "FIGHTER";
        [SerializeField] private LeaderBoardWindow _leaderBoardWindow;
        [SerializeField] private MessagePopup _messagePopup;

        private List<LeaderBoardItem> _leaderBoard;
        [CanBeNull]
        private LeaderBoardItem _playerScore;
        
        private int _value;
        
        private void Awake()
        {
            _leaderBoardWindow.SetCallbacks(LoadLeaderBoard, AddScore);
        }

        private void LoadLeaderBoard()
        {
            ArenaSDKManager.Instance.LoadLeaderBoard(_leaderboardAlias, LoadCallback);
        }

        private void LoadCallback(IResponse response)
        {
            switch (response)
            {
                case GetLeaderBoardSuccess success:
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
            var playerInfo = ArenaSDKManager.Instance.UserInfo;
            if (playerInfo == null)
            {
                _messagePopup.Show("User info not loaded. ");
                return;
            }

            _playerScore = items.Count == 0 ? new LeaderBoardItem() : items.First(it => it.profileId == playerInfo.id);
        }

        private void AddScore()
        {
            if (_playerScore == null) return;
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