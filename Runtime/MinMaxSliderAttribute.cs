using UnityEngine;

namespace ShalicoUtils
{
    public class MinMaxSliderAttribute : PropertyAttribute
    {
        public MinMaxSliderAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Min { get; }
        public float Max { get; }
    }
}