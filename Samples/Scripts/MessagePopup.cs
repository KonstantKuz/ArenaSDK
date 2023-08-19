using UnityEngine;
using UnityEngine.UI;

namespace Samples.Scripts
{
    public class MessagePopup : MonoBehaviour
    {
        [SerializeField] private Button _close;
        [SerializeField] private Text _text;

        private void Awake()
        {
            _close.onClick.AddListener(Close);
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }

        public void Show(string message)
        {
            gameObject.SetActive(true);
            _text.text = message;
        }
    }
}