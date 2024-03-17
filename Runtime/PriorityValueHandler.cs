using System.Runtime.CompilerServices;

namespace ShalicoUtils
{
    internal class PriorityValueHandler<T> : IPriorityValueHandler<T>
    {
        private static PriorityValueHandler<T> s_poolRoot;
        private PriorityValueHandler<T> _next;

        private PriorityValueArbiter<T> _arbiter;

        private int _priority;
        private T _value;

        public int Priority
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _priority;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (_priority == value)
                    return;

                _priority = value;
                _arbiter.UpdateHandlerPriority(this);
            }
        }

        public T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _value;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _value = value;
                _arbiter.UpdateHandlerValue(this);
            }
        }

        public ushort Version { get; private set; }
        public bool IsTopPriority => _arbiter.IsTopPriorityValueHandler(this);

        public void RemoveHandler()
        {
            _arbiter.RemoveHandler(this);
        }

        public static PriorityValueHandler<T> Rent(PriorityValueArbiter<T> owner, int priority, T value)
        {
            PriorityValueHandler<T> result;
            if (s_poolRoot == null)
            {
                result = new PriorityValueHandler<T>();
            }
            else
            {
                result = s_poolRoot._next;
                s_poolRoot._next = result._next;
                result._next = null;
            }

            result._arbiter = owner;
            result._priority = priority;
            result._value = value;

            return result;
        }

        public static void Return(PriorityValueHandler<T> buffer)
        {
            buffer.Version++;

            if (buffer.Version != ushort.MaxValue)
            {
                buffer._next = s_poolRoot;
                s_poolRoot = buffer;
            }
        }
    }
}