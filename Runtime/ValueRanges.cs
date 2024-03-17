using UnityEngine;

namespace ShalicoUtils
{
    public static class ValueRanges
    {
        public static readonly ValueRange<float> ZeroToOne = new(0f, 1f);
        public static readonly ValueRange<float> MinusOneToOne = new(-1f, 1f);

        public static readonly ValueRange<float> ZeroTo360 = new(0f, 360f);
        public static readonly ValueRange<float> Minus180To180 = new(-180f, 180f);
        public static readonly ValueRange<float> ZeroTo2Pi = new(0f, Mathf.PI * 2f);
        public static readonly ValueRange<float> MinusPiToPi = new(-Mathf.PI, Mathf.PI);

        public static readonly ValueRange<float> Minus80To0 = new(-80f, 0f);
    }
}