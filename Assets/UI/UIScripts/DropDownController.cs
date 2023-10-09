using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownController : MonoBehaviour
{
    [SerializeField] private DropDownElement[] _dropDowns;
    [SerializeField] private int _defaultActiveIndex = 0;

    private int _currentActiveIndex;
    private void OnValidate()
    {
        if (_dropDowns.Length - 1 < 0)
            return;

        if(_defaultActiveIndex > _dropDowns.Length - 1 && _defaultActiveIndex != 0)
        {
            Debug.LogError("Drop down index does not go to " + _defaultActiveIndex);
        }

        _currentActiveIndex = _defaultActiveIndex;
        SetActiveDropDown(_currentActiveIndex);

    }

    public void SetActiveDropDown(int activeIndex)
    {
        _currentActiveIndex = activeIndex;
        for (int i = 0; i < _dropDowns.Length; i++)
        {
            if (i == _currentActiveIndex)
            {
                _dropDowns[i].IsActive = true;
            }
            else
            {

                _dropDowns[i].IsActive = false;
            }
        }
    }



}
