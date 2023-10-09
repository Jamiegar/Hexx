using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabHandler : MonoBehaviour
{
    [Header("Tabs Style")]
    public Sprite ActiveTab;
    public Sprite InactiveTab;

    [Header("Tabs Set up")]
    [SerializeField] private Tab[] tabs;
    [SerializeField] private int defaultActiveIndex = 0;

    private int _currentActiveIndex;

    private void OnValidate()
    {
        _currentActiveIndex = defaultActiveIndex;
        SetActiveIndex(defaultActiveIndex);
    }

    public void SetActiveIndex(int activeIndex)
    {
        if (defaultActiveIndex > tabs.Length - 1 && defaultActiveIndex != 0)
        {
            Debug.LogError("Tabs Index does not go up to " + defaultActiveIndex);
        }

        if (ActiveTab != null && InactiveTab != null)
        {
            _currentActiveIndex = activeIndex;

            for (int i = 0; i < tabs.Length; i++)
            {
                if (i == _currentActiveIndex)
                {
                    tabs[i].ActiveTab = true;
                }
                else
                {

                    tabs[i].ActiveTab = false;
                }
            }
        }
    }

}
