using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudsWeatherOptionsModifier : MonoBehaviour, IInitalizable
{
    [SerializeField] private Slider _numberOfObjects, _speedOfClouds;
    [SerializeField] private Clouds _weatherSystem;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        _numberOfObjects.value = _weatherSystem.NumberOfObjects;
        _speedOfClouds.value = _weatherSystem.CloudsMovementSpeed;

        _numberOfObjects.onValueChanged.AddListener(OnNumberOfObjectsChanged);
        _speedOfClouds.onValueChanged.AddListener(OnSpeedOfCloudsChanged);
    }

    private void OnNumberOfObjectsChanged(float value)
    {
        _weatherSystem.NumberOfObjects = Mathf.RoundToInt(value);
    }

    private void OnSpeedOfCloudsChanged(float value)
    {
        _weatherSystem.CloudsMovementSpeed = value;
    }

}
