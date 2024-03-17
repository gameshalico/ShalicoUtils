using Packages.ShalicoUtils.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShalicoUtils.Editor
{
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var minMaxSliderAttribute = (MinMaxSliderAttribute)attribute;

            var valueRangeSlider = new ValueRangeSlider(
                property.displayName, property, minMaxSliderAttribute.Min, minMaxSliderAttribute.Max);

            return valueRangeSlider;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);

            var position2 = new Rect(position.x, position.y + EditorGUI.GetPropertyHeight(property, label),
                position.width,
                EditorGUIUtility.singleLineHeight);

            var propertyValue = property.GetPropertyValue();
            var minMaxAttribute = (MinMaxSliderAttribute)attribute;

            if (propertyValue is ValueRange<float> valueRange)
            {
                var range = MinMaxSlider(valueRange, position2, minMaxAttribute);
                property.SetPropertyValue(range);
            }
            else if (propertyValue is ValueRange<int> valueRangeInt)
            {
                var range = MinMaxSlider(valueRangeInt.ToFloatRange(), position2, minMaxAttribute);
                property.SetPropertyValue(range.ToIntRangeByRound());
            }
            else
            {
                EditorGUI.LabelField(position2, "",
                    "MinMaxSliderAttribute can only be used with ValueRange<float> or ValueRange<int>");
            }
        }

        private static ValueRange<float> MinMaxSlider(ValueRange<float> value, Rect position2,
            MinMaxSliderAttribute minMaxAttribute)
        {
            var min = value.Min;
            var max = value.Max;

            EditorGUI.MinMaxSlider(position2, "", ref min, ref max, minMaxAttribute.Min, minMaxAttribute.Max);

            if (max < min) max = min;
            return new ValueRange<float>(min, max);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight;
        }
    }
}