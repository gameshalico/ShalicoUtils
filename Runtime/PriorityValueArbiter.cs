using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace ShalicoUtils
{
    public class PriorityValueArbiter<T>
    {
        private readonly List<PriorityValueHandler<T>> _handlers = new();
        private T _defaultValue;
        private Action<T> _onValueChanged;

        public PriorityValueArbiter(T defaultValue = default)
        {
            _defaultValue = defaultValue;
            Value = defaultValue;
        }

        public T Value { get; private set; }

        public T DefaultValue
        {
            get => _defaultValue;
            set
            {
                _defaultValue = value;

                if (_handlers.Count == 0)
                    UpdateValue(value);
            }
        }

        public event Action<T> OnValueChanged
        {
            add => _onValueChanged += value;
            remove => _onValueChanged -= value;
        }

        [MustUseReturnValue]
        public PriorityValueHandle<T> Register(T value, int priority = 0)
        {
            var buffer = PriorityValueHandler<T>.Rent(this, priority, value);

            InsertHandler(buffer);
            UpdateValueIfTopPriorityValueHandler(buffer);

            return new PriorityValueHandle<T>(buffer);
        }

        private void InsertHandler(PriorityValueHandler<T> buffer)
        {
            var index = 0;
            for (; index < _handlers.Count; index++)
                if (_handlers[index].Priority > buffer.Priority)
                    break;

            _handlers.Insert(index, buffer);
        }

        internal void RemoveHandler(PriorityValueHandler<T> buffer)
        {
            var isTopPriorityValueHandler = IsTopPriorityValueHandler(buffer);
            _handlers.Remove(buffer);
            PriorityValueHandler<T>.Return(buffer);

            if (isTopPriorityValueHandler)
                UpdateValue(GetTopPriorityValue());
        }

        internal void UpdateHandlerPriority(PriorityValueHandler<T> buffer)
        {
            _handlers.Remove(buffer);
            InsertHandler(buffer);
            UpdateValueIfTopPriorityValueHandler(buffer);
        }

        internal void UpdateHandlerValue(PriorityValueHandler<T> buffer)
        {
            UpdateValueIfTopPriorityValueHandler(buffer);
        }

        internal bool IsTopPriorityValueHandler(PriorityValueHandler<T> buffer)
        {
            return _handlers.Count > 0 && _handlers[0] == buffer;
        }

        private void UpdateValueIfTopPriorityValueHandler(PriorityValueHandler<T> buffer)
        {
            if (!IsTopPriorityValueHandler(buffer))
                return;

            UpdateValue(buffer.Value);
        }

        private T GetTopPriorityValue()
        {
            return _handlers.Count > 0 ? _handlers[0].Value : _defaultValue;
        }

        private void UpdateValue(T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, Value))
                return;

            Value = value;
            _onValueChanged?.Invoke(Value);
        }
    }
}