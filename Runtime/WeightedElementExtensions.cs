using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace ShalicoUtils
{
    public static class WeightedElementExtensions
    {
        public static WeightedElement<T>[] ToWeightedElements<T>(this IEnumerable<ValueTuple<T, float>> elements)
        {
            var weightedElements = new List<WeightedElement<T>>();
            foreach (var (element, weight) in elements) weightedElements.Add(new WeightedElement<T>(element, weight));
            return weightedElements.ToArray();
        }

        public static float GetTotalWeight<T>(this IEnumerable<WeightedElement<T>> elements)
        {
            float totalWeight = 0;
            foreach (var element in elements) totalWeight += element.Weight;
            return totalWeight;
        }

        public static WeightedElement<T> TakeElementByWeight<T>(this IEnumerable<WeightedElement<T>> elements,
            float weight)
        {
            float currentWeight = 0;
            foreach (var element in elements)
            {
                currentWeight += element.Weight;
                if (currentWeight >= weight) return element;
            }

            throw new ArgumentOutOfRangeException(nameof(weight),
                "The weight is greater than the total weight of the elements");
        }

        public static WeightedElement<T> TakeRandomElementByWeight<T>(this WeightedElement<T>[] elements)
        {
            return elements.TakeElementByWeight(Random.Range(0, elements.GetTotalWeight()));
        }

        public static WeightedElement<T>[] TakeRandomElementsByWeight<T>(this WeightedElement<T>[] elements, int count)
        {
            var cumulativeWeights = new float[elements.Length];
            float totalWeight = 0;
            for (var i = 0; i < elements.Length; i++)
            {
                totalWeight += elements[i].Weight;
                cumulativeWeights[i] = totalWeight;
            }

            var takenElements = new WeightedElement<T>[count];
            for (var i = 0; i < count; i++)
            {
                var weight = Random.Range(0, totalWeight);
                var j = Array.BinarySearch(cumulativeWeights, weight);
                if (j < 0) j = ~j;
                takenElements[i] = elements[j];
            }

            return takenElements;
        }

        public static WeightedElement<T>[] TakeUniqueRandomElementsByWeight<T>(this WeightedElement<T>[] elements,
            int count)
        {
            if (count > elements.Length)
                throw new ArgumentOutOfRangeException(nameof(count),
                    "The count is greater than the total count of the elements");

            var cumulativeWeights = new float[elements.Length];
            float totalWeight = 0;
            for (var i = 0; i < elements.Length; i++)
            {
                totalWeight += elements[i].Weight;
                cumulativeWeights[i] = totalWeight;
            }

            var takenElements = new WeightedElement<T>[count];
            var takenIndices = new HashSet<int>();

            var index = 0;
            while (index < count)
            {
                var weight = Random.Range(0, totalWeight);
                var i = Array.BinarySearch(cumulativeWeights, weight);
                if (i < 0) i = ~i;

                if (takenIndices.Add(i))
                {
                    takenElements[index] = elements[i];
                    index++;
                }
            }

            return takenElements;
        }
    }
}