using System;
using UnityEngine;

namespace Util
{
    public static class DisposableExtension
    {
        public static IDisposable ToDisposable(this Coroutine coroutine, MonoBehaviour runner)
        {
            return new CoroutineDisposable(coroutine, runner);
        }

        public static void AddTo(this IDisposable disposable, CompositeDisposable compositeDisposable)
        {
            compositeDisposable.Add(disposable);
        }
    
        public class CoroutineDisposable : IDisposable
        {
            private readonly Coroutine _coroutine;
            private readonly MonoBehaviour _runnner;

            public CoroutineDisposable(Coroutine coroutine, MonoBehaviour runnner)
            {
                _coroutine = coroutine;
                _runnner = runnner;
            }

            public void Dispose()
            {
                _runnner.StopCoroutine(_coroutine);
            }
        }
    }
}