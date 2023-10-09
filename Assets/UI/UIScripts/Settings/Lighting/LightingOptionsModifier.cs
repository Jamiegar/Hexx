using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightingOptionsModifier : MonoBehaviour, IInitalizable
{
    [SerializeField] private Slider _timeOfDay, _seedOfCycle;

    [Header("Run Time Sets")]
    [SerializeField] private LightingManagerSet _lightingManagerSet;
    private LightingManager _lightingManager;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        _lightingManager = _lightingManagerSet.GetItemIndex(0);

        _timeOfDay.value = _lightingManager.TimeOfDay;
        _seedOfCycle.value = _lightingManager.SpeedOfCycle;

        _timeOfDay.onValueChanged.AddListener(OnTimeOfDayChanged);
        _seedOfCycle.onValueChanged.AddListener(OnSpeedOfCycleChanged);
    }

    private void OnTimeOfDayChanged(float value)
    {
        _lightingManager.TimeOfDay = value;
    }

    private void OnSpeedOfCycleChanged(float value)
    {
        _lightingManager.SpeedOfCycle = value;
    }
}
