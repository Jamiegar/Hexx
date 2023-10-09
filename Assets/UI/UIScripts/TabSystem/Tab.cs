using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    [SerializeField] private TabHandler _tabGroup;
    [SerializeField] private Image _tabBackground;
    [SerializeField] private GameObject _tabPanel;

    private bool isActiveTab;
    public bool ActiveTab 
    {
        set
        {
            isActiveTab = value;
            SetTab();
        }
        get
        {
            return isActiveTab;
        }
    }

    private void SetTab()
    {
        if (_tabGroup == null || _tabBackground == null || _tabPanel == null)
            return;

        if (ActiveTab == true)
        {
            _tabBackground.sprite = _tabGroup.ActiveTab;
            _tabPanel.SetActive(true);
        }
        else
        {
            _tabBackground.sprite = _tabGroup.InactiveTab;
            _tabPanel.SetActive(false);
        }
    }


}
