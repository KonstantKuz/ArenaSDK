using System;
using System.Collections.Generic;
using System.Linq;
using LeaderBoard.ResponseForm;
using UnityEngine;
using UnityEngine.UI;

namespace Samples.Scripts
{
    public class LeaderBoardWindow : MonoBehaviour
    {
        [SerializeField] private Transform _elementRoot;
        [SerializeField] private LeaderBoardItemView _itemViewPrefab;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _addScoreButton;

        public void SetCallbacks(Action loadCallback, Action addScoreCallback)
        {
            _loadButton.onClick.RemoveAllListeners();
            _loadButton.onClick.AddListener(() => loadCallback?.Invoke());
            _addScoreButton.onClick.RemoveAllListeners();
            _addScoreButton.onClick.AddListener(() => addScoreCallback?.Invoke());
        }
        
        public void UpdateView(List<LeaderBoardItem> items)
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