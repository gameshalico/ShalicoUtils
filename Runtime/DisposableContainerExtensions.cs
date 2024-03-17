using System;

namespace ShalicoUtils
{
    public static class DisposableContainerExtensions
    {
        public static void AddTo(this IDisposable disposable, DisposableContainer container)
        {
            container.Add(disposable);
        }
    }
}