using LeaderBoard;
using Response;
using Response.Fail;
using UnityEngine;

namespace Samples.Scripts
{
    public class LeaderBoardSample : MonoBehaviour
    {
        [SerializeField] private string _leaderboardAlias = "FIGHTER";
        [SerializeField] private LeaderBoardWindow _leaderBoardWindow;
        [SerializeField] private MessagePopup _messagePopup;
        
        private void Awake()
        {
            _leaderBoardWindow.LoadButton.onClick.AddListener(LoadLeaderBoard);
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
                    _leaderBoardWindow.Init(success.leaderboards);
                    break;
                case IFailResponse fail:
                    _messagePopup.Show(fail.Message);
                    break;
            }
        }
    }
}