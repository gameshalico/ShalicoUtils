using Packages.ShalicoUtils.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShalicoUtils.Editor
{
    public class ValueRangeSlider : VisualElement
    {
        private readonly FloatField _minField;
        private readonly FloatField _maxField;
        private readonly MinMaxSlider _slider;
        private readonly SerializedProperty _property;
        private readonly bool _isInt;

        public ValueRangeSlider(string label, SerializedProperty property, float sliderMin, float sliderMax)
        {
            _property = property;
            var value = property.GetPropertyValue();

            var sliderValue = ValueRange<float>.Empty;
            if (value is ValueRange<float> floatRange)
            {
                sliderValue = floatRange;
            }
            else if (value is ValueRange<int> intRange)
            {
                sliderValue = intRange.ToFloatRange();
                _isInt = true;
            }
            else
            {
                Debug.LogError("ValueRangeSlider can only be used with ValueRange<float> or ValueRange<int>");
            }

            _minField = new FloatField();
            _minField.SetValueWithoutNotify(sliderValue.Min);
            _minField.RegisterValueChangedCallback(OnMinFieldChanged);

            _maxField = new FloatField();
            _maxField.SetValueWithoutNotify(sliderValue.Max);
            _maxField.RegisterValueChangedCallback(OnMaxFieldChanged);

            _slider = new MinMaxSlider("", sliderValue.Min, sliderValue.Max, sliderMin, sliderMax);
            _slider.RegisterValueChangedCallback(OnSliderValueChanged);

            var sliderContainer = new VisualElement();

            _minField.style.width = 50;
            _minField.style.marginRight = 10;
            _maxField.style.width = 50;
            _maxField.style.marginLeft = 10;
            _slider.style.flexGrow = 3;
            sliderContainer.style.flexDirection = FlexDirection.Row;

            sliderContainer.Add(_minField);
            sliderContainer.Add(_slider);
            sliderContainer.Add(_maxField);

            var labelElement = new Label(label);

            style.flexDirection = FlexDirection.Row;

            StyleUtility.UnityFieldContainerStyle(style);
            StyleUtility.UnityFieldLabelStyle(labelElement.style);
            StyleUtility.UnityFieldValueStyle(sliderContainer.style);

            Add(labelElement);
            Add(sliderContainer);
        }

        private void OnSliderValueChanged(ChangeEvent<Vector2> evt)
        {
            var valueRange = evt.newValue.ToValueRange();
            if (_isInt) valueRange = valueRange.ToIntRangeByRound().ToFloatRange();

            _minField.SetValueWithoutNotify(valueRange.Min);
            _maxField.SetValueWithoutNotify(valueRange.Max);
            _slider.SetValueWithoutNotify(valueRange.ToVector2());

            UpdatePropertyBySliderValue();
        }

        private void OnMinFieldChanged(ChangeEvent<float> evt)
        {
            var min = evt.newValue;
            if (_isInt) min = Mathf.Round(min);
            if (min > _maxField.value) min = _maxField.value;

            _minField.SetValueWithoutNotify(min);
            _slider.SetValueWithoutNotify(new Vector2(min, _slider.maxValue));
            UpdatePropertyBySliderValue();
        }

        private void OnMaxFieldChanged(ChangeEvent<float> evt)
        {
            var max = evt.newValue;
            if (_isInt) max = Mathf.Round(max);
            if (max < _minField.value) max = _minField.value;

            _maxField.SetValueWithoutNotify(max);
            _slider.SetValueWithoutNotify(new Vector2(_slider.minValue, max));
            UpdatePropertyBySliderValue();
        }

        private void UpdatePropertyBySliderValue()
        {
            var newValue = new ValueRange<float>(_slider.minValue, _slider.maxValue);

            if (_isInt)
                _property.SetPropertyValue(newValue.ToIntRangeByRound());
            else
                _property.SetPropertyValue(newValue);
        }
    }
}