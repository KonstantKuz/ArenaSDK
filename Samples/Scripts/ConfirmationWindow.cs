using UnityEngine;
using UnityEngine.UI;

namespace Samples.Scripts
{
    public class ConfirmationWindow : MonoBehaviour
    {
        [field: SerializeField] public InputField Code;
        [field: SerializeField] public Button Confirm;
        [field: SerializeField] public Button SendCode;
    }
}