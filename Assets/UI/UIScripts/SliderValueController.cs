using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GD.MinMaxSlider;
using TMPro;

public class SliderValueController : MonoBehaviour, IInitalizable
{
    [SerializeField, MinMaxSlider(0, 1000)] private Vector2 _minMaxValue;
    [SerializeField] private bool _forceOnDecimalPlace = true;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _textValue;

    private void OnValidate()
    {
        Init();
        _slider.minValue = _minMaxValue.x;
        _slider.maxValue = _minMaxValue.y;
        _slider.value = _minMaxValue.y / 2f;
    }

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        if (_slider == null)
            _slider = GetComponent<Slider>();

        if (_textValue == null)
            _textValue = GetComponentInChildren<TextMeshProUGUI>();

        _slider.onValueChanged.AddListener(OnSliderValueChanged);

        OnSliderValueChanged(_slider.value);
    }

    public void OnSliderValueChanged(float value)
    {
        if(_forceOnDecimalPlace == true)
            _textValue.text = value.ToString("F0");
        else
        {
            _textValue.text = value.ToString("F1");
        }
    }


}
