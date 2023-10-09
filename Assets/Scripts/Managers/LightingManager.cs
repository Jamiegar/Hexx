using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light m_directionalLight;
    [SerializeField] private LightingPreset m_preset;
    [SerializeField, Range(0, 24)] private float m_timeOfDay;
    [SerializeField, Range(0f, 1f)] private float m_speedOfCycle;

    public float TimeOfDay
    {
        get{  return m_timeOfDay; }
        set{  m_timeOfDay = value; }
    }

    public float SpeedOfCycle
    {
        get { return m_speedOfCycle; }
        set { m_speedOfCycle = value; }
    }


    private void Update()
    {
        if (m_preset == null)
            return;

        if(Application.isPlaying)
        {
            m_timeOfDay += Time.deltaTime * m_speedOfCycle;
            m_timeOfDay %= 24; //This clamps the time between 0 and 24
            UpdateLighting(m_timeOfDay / 24f);
        }
        else
        {
            UpdateLighting(m_timeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = m_preset.AmbientColour.Evaluate(timePercent);
        RenderSettings.fogColor = m_preset.FogColour.Evaluate(timePercent);

        if(m_directionalLight != null)
        {
            m_directionalLight.color = m_preset.DirectionalColour.Evaluate(timePercent);
            m_directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }

    private void OnValidate()
    {
        if (m_directionalLight != null)
            return;

        if(RenderSettings.sun != null)
        {
            m_directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    m_directionalLight = light;
                    return;
                }
            }
        }
    }

}
