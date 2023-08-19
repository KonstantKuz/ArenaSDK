using System;
using System.Collections;

namespace Tests.Runtime
{
    public static class CoroutineHelper
    {
        public class CoroutineTimeoutException : Exception
        {
            public CoroutineTimeoutException(string message) : base(message) { }
        }
 
        public static void RunSynchronously(IEnumerator coroutine, int maxNumberOfCalls = 1000)
        {
            while (coroutine.MoveNext())
            {
                maxNumberOfCalls--;
                if (maxNumberOfCalls < 0)
                    throw new CoroutineTimeoutException("Coroutine reached maximum number of calls: " + maxNumberOfCalls);
            }
        }
    }
}