using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ShalicoUtils
{
    public static class ValueRangeFloatExtensions
    {
        public static ValueRange<int> ToIntRange(this ValueRange<float> range)
        {
            return new ValueRange<int>((int)range.Min, (int)range.Max);
        }

        public static ValueRange<int> ToIntRangeByRound(this ValueRange<float> range)
        {
            return new ValueRange<int>(Mathf.RoundToInt(range.Min), Mathf.RoundToInt(range.Max));
        }

        public static ValueRange<int> ToIntRangeByCeil(this ValueRange<float> range)
        {
            return new ValueRange<int>(Mathf.CeilToInt(range.Min), Mathf.CeilToInt(range.Max));
        }

        public static ValueRange<int> ToIntRangeByFloor(this ValueRange<float> range)
        {
            return new ValueRange<int>(Mathf.FloorToInt(range.Min), Mathf.FloorToInt(range.Max));
        }

        public static ValueRange<double> ToDoubleRange(this ValueRange<float> range)
        {
            return new ValueRange<double>(range.Min, range.Max);
        }

        public static ValueRange<float> ToValueRange(this Vector2 vector)
        {
            return new ValueRange<float>(vector.x, vector.y);
        }

        public static Vector2 ToVector2(this ValueRange<float> range)
        {
            return new Vector2(range.Min, range.Max);
        }

        public static float Length(this ValueRange<float> range)
        {
            return range.Max - range.Min;
        }

        public static IEnumerable<float> Enumerate(this ValueRange<float> range, float step = 1)
        {
            for (var i = range.Min; i < range.Max; i += step) yield return i;
        }

        public static IEnumerable<ValueRange<float>> SplitByCount(this ValueRange<float> range, int count)
        {
            var step = range.Length() / count;
            return range.Enumerate(step).Select(x => new ValueRange<float>(x, x + step));
        }

        public static IEnumerable<ValueRange<float>> SplitByStep(this ValueRange<float> range, float step)
        {
            return range.Enumerate(step).Select(x => new ValueRange<float>(x, x + step));
        }

        public static float Random(this ValueRange<float> range)
        {
            return UnityEngine.Random.Range(range.Min, range.Max);
        }

        public static float Median(this ValueRange<float> range)
        {
            return range.Min + range.Length() / 2;
        }

        public static ValueRange<float> Shift(this ValueRange<float> range, float shift)
        {
            return new ValueRange<float>(range.Min + shift, range.Max + shift);
        }

        public static ValueRange<float> ExpandBy(this ValueRange<float> range, float expand)
        {
            if (expand < 0) throw new ArgumentException("expand must be greater than or equal to 0", nameof(expand));

            return new ValueRange<float>(range.Min - expand, range.Max + expand);
        }

        public static ValueRange<float> ShrinkBy(this ValueRange<float> range, float shrink)
        {
            if (shrink < 0) throw new ArgumentException("shrink must be greater than or equal to 0", nameof(shrink));

            if (range.Length() < shrink * 2)
            {
                var median = range.Median();
                return new ValueRange<float>(median, median);
            }

            return new ValueRange<float>(range.Min + shrink, range.Max - shrink);
        }

        public static float Remap(this ValueRange<float> from, ValueRange<float> to, float value)
        {
            return to.Min + (value - from.Min) * to.Length() / from.Length();
        }

        public static float Lerp(this ValueRange<float> range, float t)
        {
            return range.Min + range.Length() * t;
        }

        public static float Normalize(this ValueRange<float> range, float value)
        {
            return (value - range.Min) / range.Length();
        }
    }
}