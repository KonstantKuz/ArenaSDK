using UnityEngine;
using UnityEngine.UI;

namespace Samples.Scripts
{
    public class AuthorizationWindow : MonoBehaviour
    {
        [field:SerializeField] public InputField Login;
        [field:SerializeField] public InputField Password;
        [field: SerializeField] public Button Confirm;
    }
}