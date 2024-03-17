using System;

namespace ShalicoUtils
{
    [Serializable]
    public struct WeightedElement<T>
    {
        public T Element;
        public float Weight;

        public WeightedElement(T element, float weight)
        {
            Element = element;
            Weight = weight;
        }

        public static implicit operator T(WeightedElement<T> weightedElement)
        {
            return weightedElement.Element;
        }

        public static implicit operator WeightedElement<T>(T element)
        {
            return new WeightedElement<T>(element, 1);
        }

        public static implicit operator WeightedElement<T>(ValueTuple<T, float> tuple)
        {
            return new WeightedElement<T>(tuple.Item1, tuple.Item2);
        }

        public static implicit operator ValueTuple<T, float>(WeightedElement<T> weightedElement)
        {
            return (weightedElement.Element, weightedElement.Weight);
        }

        public override string ToString()
        {
            return $"{Element} ({Weight})";
        }
    }
}