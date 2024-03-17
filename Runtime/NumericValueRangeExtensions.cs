using System;
using System.Collections.Generic;

namespace ShalicoUtils
{
    public static class NumericValueRangeExtensions
    {
        public static T Length<T>(this ValueRange<T> range)
            where T : struct, IComparable<T>, IConvertible
        {
            var doubleMin = range.Min.ToDouble(null);
            var doubleMax = range.Max.ToDouble(null);
            return (T)Convert.ChangeType(doubleMax - doubleMin, typeof(T));
        }

        public static IEnumerable<T> Enumerate<T>(this ValueRange<T> range, T step)
            where T : struct, IComparable<T>, IConvertible
        {
            var doubleValue = range.Min.ToDouble(null);
            var doubleMax = range.Max.ToDouble(null);
            var doubleStep = step.ToDouble(null);
            while (doubleValue < doubleMax)
            {
                yield return (T)Convert.ChangeType(doubleValue, typeof(T));
                doubleValue += doubleStep;
            }
        }

        public static ValueRange<T1> ConvertTo<T0, T1>(this ValueRange<T0> range)
            where T0 : struct, IComparable<T0>, IConvertible
            where T1 : struct, IComparable<T1>, IConvertible
        {
            return new ValueRange<T1>((T1)Convert.ChangeType(range.Min, typeof(T1)),
                (T1)Convert.ChangeType(range.Max, typeof(T1)));
        }

        public static double Remap<T>(this ValueRange<T> range, ValueRange<T> from, T value)
            where T : struct, IComparable<T>, IConvertible
        {
            var doubleMin = range.Min.ToDouble(null);
            var doubleMax = range.Max.ToDouble(null);
            var doubleValue = value.ToDouble(null);
            var doubleFromMin = from.Min.ToDouble(null);
            var doubleFromMax = from.Max.ToDouble(null);
            return doubleMin + (doubleValue - doubleFromMin) / (doubleFromMax - doubleFromMin) *
                (doubleMax - doubleMin);
        }

        public static T Lerp<T>(this ValueRange<T> range, double t) where T : struct, IComparable<T>, IConvertible
        {
            var doubleMin = range.Min.ToDouble(null);
            var doubleMax = range.Max.ToDouble(null);
            var doubleValue = doubleMin + (doubleMax - doubleMin) * t;
            return (T)Convert.ChangeType(doubleValue, typeof(T));
        }

        public static T Random<T>(this ValueRange<T> range) where T : struct, IComparable<T>, IConvertible
        {
            var doubleMin = range.Min.ToDouble(null);
            var doubleMax = range.Max.ToDouble(null);
            double doubleValue = UnityEngine.Random.Range((float)doubleMin, (float)doubleMax);
            return (T)Convert.ChangeType(doubleValue, typeof(T));
        }

        public static T Median<T>(this ValueRange<T> range) where T : struct, IComparable<T>, IConvertible
        {
            var doubleMin = range.Min.ToDouble(null);
            var doubleMax = range.Max.ToDouble(null);
            var doubleValue = doubleMin + (doubleMax - doubleMin) * 0.5;
            return (T)Convert.ChangeType(doubleValue, typeof(T));
        }

        public static ValueRange<T> Shift<T>(this ValueRange<T> range, T shift)
            where T : struct, IComparable<T>, IConvertible
        {
            var doubleMin = range.Min.ToDouble(null);
            var doubleMax = range.Max.ToDouble(null);
            var doubleShift = shift.ToDouble(null);
            return new ValueRange<T>((T)Convert.ChangeType(doubleMin + doubleShift, typeof(T)),
                (T)Convert.ChangeType(doubleMax + doubleShift, typeof(T)));
        }

        public static ValueRange<T>[] SplitByCount<T>(this ValueRange<T> range, int count)
            where T : struct, IComparable<T>, IConvertible
        {
            var result = new ValueRange<T>[count];
            var doubleMin = range.Min.ToDouble(null);
            var doubleMax = range.Max.ToDouble(null);
            var doubleStep = (doubleMax - doubleMin) / count;
            for (var i = 0; i < count; i++)
            {
                var doubleValue = doubleMin + doubleStep * i;
                result[i] = new ValueRange<T>((T)Convert.ChangeType(doubleValue, typeof(T)),
                    (T)Convert.ChangeType(doubleValue + doubleStep, typeof(T)));
            }

            return result;
        }

        public static ValueRange<T>[] SplitByStep<T>(this ValueRange<T> range, T step)
            where T : struct, IComparable<T>, IConvertible
        {
            var doubleMin = range.Min.ToDouble(null);
            var doubleMax = range.Max.ToDouble(null);
            var doubleStep = step.ToDouble(null);
            var count = (int)((doubleMax - doubleMin) / doubleStep);
            var result = new ValueRange<T>[count];
            for (var i = 0; i < count; i++)
            {
                var doubleValue = doubleMin + doubleStep * i;
                result[i] = new ValueRange<T>((T)Convert.ChangeType(doubleValue, typeof(T)),
                    (T)Convert.ChangeType(doubleValue + doubleStep, typeof(T)));
            }

            return result;
        }
    }
}