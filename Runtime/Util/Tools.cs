using UnityEditor;
using UnityEngine;

namespace Util
{
    public static class Tools
    {
        [MenuItem("Tools/SignIn")]
        public static void SignIn()
        {
            ArenaSDKManager.Instance.AuthorizeUser("stalkercatkuz", "controller-vs-krovosos090997", Debug.Log);
        }
    }
}