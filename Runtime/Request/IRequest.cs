using System.Collections;
using UnityEngine.Networking;

namespace Request
{
    public interface IRequest
    {
        public UnityWebRequest Body { get; }
        public object Result { get; }
        public IEnumerator Send();
    }
}