using System.Collections.Generic;
using System.Linq;
using LeaderBoard;
using UnityEngine;
using UnityEngine.UI;

namespace Samples.Scripts
{
    public class LeaderBoardWindow : MonoBehaviour
    {
        [SerializeField] private Transform _elementRoot;
        [SerializeField] private LeaderBoardItemView _itemViewPrefab;
        [field:SerializeField] public Button LoadButton { get; private set; }

        public void Init(List<LeaderBoardItem> items)
        {
            Clear();
            foreach (var leaderBoardItem in items.OrderBy(it => it.position))
            {
                CreateItem(leaderBoardItem);
            }
        }

        private void CreateItem(LeaderBoardItem item)
        {
            var itemView = Instantiate(_itemViewPrefab);
            itemView.Init(item);
        }

        private void Clear()
        {
            foreach (var element in _elementRoot.GetComponentsInChildren<LeaderBoardItemView>())
            {
                Destroy(element);
            }
        }
    }
}