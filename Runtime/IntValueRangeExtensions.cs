using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ShalicoUtils
{
    public static class IntValueRangeExtensions
    {
        public static ValueRange<float> ToFloatRange(this ValueRange<int> range)
        {
            return new ValueRange<float>(range.Min, range.Max);
        }

        public static ValueRange<double> ToDoubleRange(this ValueRange<int> range)
        {
            return new ValueRange<double>(range.Min, range.Max);
        }

        public static RangeInt ToRangeInt(this ValueRange<int> range)
        {
            return new RangeInt(range.Min, range.Max);
        }

        public static ValueRange<int> ToValueRange(this RangeInt range)
        {
            return new ValueRange<int>(range.start, range.end);
        }

        public static Vector2 ToVector2(this ValueRange<int> range)
        {
            return new Vector2(range.Min, range.Max);
        }

        public static Vector2Int ToVector2Int(this ValueRange<int> range)
        {
            return new Vector2Int(range.Min, range.Max);
        }

        public static int Length(this ValueRange<int> range)
        {
            return range.Max - range.Min;
        }

        public static IEnumerable<int> Enumerate(this ValueRange<int> range, int step = 1)
        {
            for (var i = range.Min; i < range.Max; i += step) yield return i;
        }

        public static IEnumerable<ValueRange<int>> SplitByCount(this ValueRange<int> range, int count)
        {
            var step = range.Length() / count;
            return range.Enumerate(step).Select(x => new ValueRange<int>(x, x + step));
        }

        public static IEnumerable<ValueRange<int>> SplitByStep(this ValueRange<int> range, int step)
        {
            return range.Enumerate(step).Select(x => new ValueRange<int>(x, x + step));
        }

        public static int Random(this ValueRange<int> range)
        {
            return UnityEngine.Random.Range(range.Min, range.Max);
        }

        public static int Median(this ValueRange<int> range)
        {
            return range.Min + range.Length() / 2;
        }

        public static ValueRange<int> Shift(this ValueRange<int> range, int shift)
        {
            return new ValueRange<int>(range.Min + shift, range.Max + shift);
        }

        public static ValueRange<int> ExpandBy(this ValueRange<int> range, int expand)
        {
            if (expand < 0) throw new ArgumentException("expand must be greater than or equal to 0", nameof(expand));

            return new ValueRange<int>(range.Min - expand, range.Max + expand);
        }

        public static ValueRange<int> ShrinkBy(this ValueRange<int> range, int shrink)
        {
            if (shrink < 0) throw new ArgumentException("shrink must be greater than or equal to 0", nameof(shrink));

            if (range.Length() < shrink * 2)
            {
                var median = range.Median();
                return new ValueRange<int>(median, median);
            }

            return new ValueRange<int>(range.Min + shrink, range.Max - shrink);
        }
    }
}