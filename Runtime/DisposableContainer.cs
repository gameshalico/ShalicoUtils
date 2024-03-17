using System;
using System.Collections.Generic;
using System.Threading;

namespace ShalicoUtils
{
    public class DisposableContainer : IDisposable
    {
        private readonly LinkedList<IDisposable> _disposables = new();
        private readonly CancellationTokenSource _onDisposeCts = new();

        public CancellationToken CancellationTokenOnDispose => _onDisposeCts.Token;

        public void Dispose()
        {
            foreach (var disposable in _disposables) disposable.Dispose();
            _onDisposeCts.Cancel();
        }

        public void Add(IDisposable disposable)
        {
            _disposables.AddLast(disposable);
        }
    }
}