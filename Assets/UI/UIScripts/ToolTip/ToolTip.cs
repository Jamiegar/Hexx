using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolTip : MonoBehaviour
{
    private Camera _camera;
    private TMP_Text _tipText;
    private RectTransform _uiTransform;

    Input_PlayerControls inputActions;

    public void Awake()
    {
        _uiTransform = gameObject.GetComponent<RectTransform>();
        _tipText = GetComponentInChildren<TMP_Text>();
        
    }

    private void Update()
    {
        if(_camera == null)
            _camera = GameManager.instance.Player.GetComponentInChildren<Camera>();
        else
        {
            Vector2 mousePosition = inputActions.Mouse.MousePosition.ReadValue<Vector2>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), mousePosition, _camera, out Vector2 localPoint);

            transform.localPosition = localPoint;
        }
    }

    private void DisplayToolTip(string displayMessage)
    {
        gameObject.SetActive(true);
        _tipText.text = displayMessage;

        _uiTransform.sizeDelta = new Vector2(_tipText.preferredWidth, _tipText.preferredHeight);
    }

    private void HideToolTip()
    {
        gameObject.SetActive(false);
    }


    private void EnableMousePosition()
    {
        if (inputActions == null)
        {
            inputActions = new Input_PlayerControls();
            inputActions.Enable();
        }
        else
            inputActions.Enable();
    }

    private void DisableMousPosition()
    {
        if (inputActions == null)
        {
            inputActions = new Input_PlayerControls();
            inputActions.Disable();
        }
        else
            inputActions.Disable();
    }

    private void OnEnable()
    {
        EnableMousePosition();
         
    }

    private void OnDisable()
    {
        DisableMousPosition();
    }
}
