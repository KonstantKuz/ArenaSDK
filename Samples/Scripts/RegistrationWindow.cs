using UnityEngine;
using UnityEngine.UI;

namespace Samples.Scripts
{
    public class RegistrationWindow : MonoBehaviour
    {
        [field:SerializeField] public InputField Email;
        [field:SerializeField] public InputField UserName;
        [field:SerializeField] public InputField Password;
        [field: SerializeField] public Button Confirm;
    }
}