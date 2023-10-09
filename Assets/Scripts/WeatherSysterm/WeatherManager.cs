using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [SerializeField]
    private List<WeatherCondition> weatherConditions;

    [SerializeField]
    private float m_weatherTickRate = 0.2f;

    private bool gameRunning = false;


    public void StartWeatherCycle()
    {
        foreach(WeatherCondition weather in weatherConditions)
        {
            weather.gameObject.transform.position = GameManager.instance.Player.transform.position;
            weather.gameObject.transform.position += new Vector3(0, 25, 0);
            weather.transform.SetParent(GameManager.instance.Player.GetComponentInChildren<TopDown_Movement>().gameObject.transform);
            weather.InitaliseWeather();
        }
        gameRunning = true;

        StartCoroutine(WeatherTick());
    }

    private IEnumerator WeatherTick()
    {
        while (gameRunning == true)
        {
            foreach (WeatherCondition weather in weatherConditions)
            {
                weather.UpdateWeather();
            }

            yield return new WaitForSeconds(m_weatherTickRate);
        }
    }
}
