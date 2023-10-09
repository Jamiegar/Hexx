using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddWeatherManagerSet : AddRunTimeSet<WeatherManagerSet>
{
    protected override void OnDisable()
    {
        runTimeSet.RemoveFromList(GetComponent<WeatherManager>());
    }

    protected override void OnEnable()
    {
        runTimeSet.AddToList(GetComponent<WeatherManager>());
    }
}
