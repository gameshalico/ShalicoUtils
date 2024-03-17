using System;
using System.Runtime.CompilerServices;

namespace ShalicoUtils
{
    public interface IPriorityValueHandler<T>
    {
        public ushort Version { get; }
        T Value { get; set; }
        int Priority { get; set; }
        bool IsTopPriority { get; }

        void RemoveHandler();
    }

    public readonly struct PriorityValueHandle<T> : IDisposable
    {
        private readonly IPriorityValueHandler<T> _buffer;
        private readonly int _version;

        internal PriorityValueHandle(IPriorityValueHandler<T> buffer)
        {
            _buffer = buffer;
            _version = buffer.Version;
        }

        public T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.Value;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckBuffer();
                _buffer.Value = value;
            }
        }

        public int Priority
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.Priority;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                CheckBuffer();
                _buffer.Priority = value;
            }
        }

        public bool IsTopPriority
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                CheckBuffer();
                return _buffer.IsTopPriority;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckBuffer()
        {
            if (_buffer == null || _buffer.Version != _version)
                throw new InvalidOperationException("The PriorityValueHandler is no longer valid");
        }

        public void Dispose()
        {
            if (_buffer == null)
                return;
            if (_buffer.Version != _version)
                return;

            _buffer.RemoveHandler();
        }
    }
}