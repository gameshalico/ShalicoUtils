using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ShalicoUtils
{
    [Serializable]
    public struct ValueRange<T> : IEquatable<ValueRange<T>> where T : struct, IComparable<T>
    {
        [SerializeField] private T _min;
        [SerializeField] private T _max;

        public ValueRange(T min, T max)
        {
            _min = min;
            _max = max;

            if (min.CompareTo(max) > 0) throw new ArgumentException("min must be less than or equal to max");
        }

        public ValueRange(ValueRange<T> other)
        {
            _min = other._min;
            _max = other._max;
        }

        public T Min => _min;
        public T Max => _max;

        public static ValueRange<T> Empty => new(default, default);

        public readonly bool Equals(ValueRange<T> other)
        {
            return EqualityComparer<T>.Default.Equals(_min, other._min) &&
                   EqualityComparer<T>.Default.Equals(_max, other._max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T EvaluateMin(T a, T b)
        {
            return a.CompareTo(b) < 0 ? a : b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T EvaluateMax(T a, T b)
        {
            return a.CompareTo(b) > 0 ? a : b;
        }

        /// <summary>
        ///     valueが範囲内にあるか判定する
        /// </summary>
        public readonly bool Contains(T value)
        {
            return _min.CompareTo(value) <= 0 && value.CompareTo(_max) <= 0;
        }

        /// <summary>
        ///     otherの範囲を包含するか判定する
        /// </summary>
        public readonly bool Contains(ValueRange<T> other)
        {
            return _min.CompareTo(other._min) <= 0 && other._max.CompareTo(_max) <= 0;
        }

        /// <summary>
        ///     otherと重なるか判定する
        /// </summary>
        public readonly bool Overlaps(ValueRange<T> other)
        {
            return _min.CompareTo(other._max) <= 0 && other._min.CompareTo(_max) <= 0;
        }

        /// <summary>
        ///     valueを範囲内に収める
        /// </summary>
        public readonly T Clamp(T value)
        {
            if (value.CompareTo(_min) < 0) return _min;
            if (value.CompareTo(_max) > 0) return _max;
            return value;
        }

        /// <summary>
        ///     otherと合成した範囲を返す(和)
        /// </summary>
        public readonly ValueRange<T> Union(ValueRange<T> other)
        {
            return Union(this, other);
        }

        /// <summary>
        ///     otherと重なる範囲を返す(積)
        /// </summary>
        public readonly ValueRange<T> Intersect(ValueRange<T> other)
        {
            return Intersect(this, other);
        }

        /// <summary>
        ///     otherの範囲を除外した範囲を返す
        /// </summary>
        public readonly IEnumerable<ValueRange<T>> Subtract(ValueRange<T> other)
        {
            return Subtract(this, other);
        }


        /// <summary>
        ///     valueを含むように範囲を拡張する
        /// </summary>
        public readonly ValueRange<T> Expand(T value)
        {
            return Expand(this, value);
        }

        /// <summary>
        ///     aとbの和となる範囲を返す
        /// </summary>
        public static ValueRange<T> Union(ValueRange<T> a, ValueRange<T> b)
        {
            return new ValueRange<T>(
                EvaluateMin(a._min, b._min),
                EvaluateMax(a._max, b._max)
            );
        }

        /// <summary>
        ///     aとbの積となる範囲を返す
        /// </summary>
        public static ValueRange<T> Intersect(ValueRange<T> a, ValueRange<T> b)
        {
            var max = EvaluateMin(a._max, b._max);
            var min = EvaluateMax(a._min, b._min);

            if (max.CompareTo(min) < 0) return Empty;

            return new ValueRange<T>(min, max);
        }

        /// <summary>
        ///     aからbを除外した範囲を返す
        /// </summary>
        public static IEnumerable<ValueRange<T>> Subtract(ValueRange<T> a, ValueRange<T> b)
        {
            // aがbに完全に含まれている場合
            if (b.Contains(a)) yield break;

            // 重なりがない場合
            if (!b.Overlaps(a))
            {
                yield return a;
            }
            else
            {
                // 重なっているか、bがaに完全に含まれている場合
                if (a._min.CompareTo(b._min) < 0) // a.min < b.min
                    yield return new ValueRange<T>(a._min, b._min);

                if (b._max.CompareTo(a._max) < 0) // b.max < a.max
                    yield return new ValueRange<T>(b._max, a._max);
            }
        }

        /// <summary>
        ///     valueを含むように範囲を拡張する
        /// </summary>
        public static ValueRange<T> Expand(ValueRange<T> range, T value)
        {
            return new ValueRange<T>(
                EvaluateMin(range._min, value),
                EvaluateMax(range._max, value)
            );
        }

        public static ValueRange<T> operator |(ValueRange<T> a, ValueRange<T> b)
        {
            return Union(a, b);
        }

        public static ValueRange<T> operator &(ValueRange<T> a, ValueRange<T> b)
        {
            return Intersect(a, b);
        }

        public static implicit operator ValueRange<T>((T min, T max) tuple)
        {
            return new ValueRange<T>(tuple.min, tuple.max);
        }

        public static implicit operator (T min, T max)(ValueRange<T> range)
        {
            return (range._min, range._max);
        }

        /// <summary>
        ///     min=maxか判定する
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsEmpty()
        {
            return _min.CompareTo(_max) == 0;
        }

        public static ValueRange<T> FromEnumerable(IEnumerable<T> enumerable)
        {
            using var enumerator = enumerable.GetEnumerator();
            var min = enumerator.Current;
            var max = enumerator.Current;

            while (enumerator.MoveNext())
            {
                min = EvaluateMin(min, enumerator.Current);
                max = EvaluateMax(max, enumerator.Current);
            }

            return new ValueRange<T>(min, max);
        }

        public readonly void Deconstruct(out T minValue, out T maxValue)
        {
            minValue = _min;
            maxValue = _max;
        }

        public static ValueRange<T> operator +(ValueRange<T> range, T value)
        {
            return Expand(range, value);
        }

        public static bool operator ==(ValueRange<T> a, ValueRange<T> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ValueRange<T> a, ValueRange<T> b)
        {
            return !a.Equals(b);
        }

        public readonly override bool Equals(object obj)
        {
            return obj is ValueRange<T> other && Equals(other);
        }

        public readonly override string ToString()
        {
            return $"[{_min}, {_max}]";
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(_min, _max);
        }

        public readonly T[] ToArray()
        {
            return new[] { _min, _max };
        }

        public static ValueRange<T> FromArray(T[] array)
        {
            return new ValueRange<T>(array[0], array[1]);
        }
    }
}