using System;
using System.Collections.Generic;

namespace Util
{
    public class CompositeDisposable : IDisposable
    {
        private List<IDisposable> _disposables = new();
        public void Add(IDisposable disposable) => _disposables.Add(disposable);
        public void Dispose()
        {
            _disposables.ForEach(it => it.Dispose());
            _disposables.Clear();
        }
    }
}