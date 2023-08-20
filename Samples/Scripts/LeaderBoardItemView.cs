using LeaderBoard;
using UnityEngine;
using UnityEngine.UI;

namespace Samples.Scripts
{
    public class LeaderBoardItemView : MonoBehaviour
    {
        [SerializeField] private Text _username;
        [SerializeField] private Text _score;

        public void Init(LeaderBoardItem item)
        {
            _username.text = item.username;
            _score.text = item.score.ToString();
        }
    }
}