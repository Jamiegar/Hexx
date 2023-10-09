using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeatherCondition : MonoBehaviour
{
    public abstract void InitaliseWeather();
    public abstract void UpdateWeather();
}
