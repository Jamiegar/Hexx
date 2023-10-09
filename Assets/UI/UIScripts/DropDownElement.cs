using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownElement : MonoBehaviour
{
    [SerializeField] private Button _hideDropDownButton, _showDropDownButton;
    [SerializeField] private GameObject _content;
    private bool isActive;

    public bool IsActive 
    {
        set
        {
            isActive = value;
            SetDropDown();
        }

        get
        {
            return isActive;
        }
    }

    private void SetDropDown()
    {
        if(isActive == true)
        {
            _showDropDownButton.gameObject.SetActive(false);
            _hideDropDownButton.gameObject.SetActive(true);
            _content.SetActive(true);
        }
        else
        {
            _showDropDownButton.gameObject.SetActive(true);
            _hideDropDownButton.gameObject.SetActive(false);
            _content.SetActive(false);
        }
    }

}
