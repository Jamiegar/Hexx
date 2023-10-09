using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownCharacter : Character<Input_PlayerControls>, Input_PlayerControls.IPlayer_MapActions
{
    [SerializeField]
    protected TopDown_Movement m_movement;

    private bool move = false;
    private bool canMove = true;

    private void Awake()
    {
        m_canMove = false;
        m_controlledActions = new Input_PlayerControls();
        m_controlledActions.Player_Map.SetCallbacks(this);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (canMove == false)
            return;

        if (context.performed)
            move = true;
        else if (context.canceled)
            move = false;
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (canMove == false)
            return;

        Vector2 input = context.action.ReadValue<Vector2>();
        m_movement.HandelCameraZoom(input);
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        PauseMenu pauseMenu = GameManager.instance.UIManager.PauseMenu;
        pauseMenu.ApplicationToogledPause.AddListener(OnPause);

        
        pauseMenu.TogglePause();
    }

    private void Update()
    {
        if(move == true)
        {
            Vector2 inputValue = m_controlledActions.Player_Map.Movement.ReadValue<Vector2>();
            m_movement.HandelMovement(inputValue);
        }

        m_movement.HandelCameraPosition();
    }

    private void OnPause(bool isPaused)
    {
        PauseMenu pauseMenu = GameManager.instance.UIManager.PauseMenu;
        if (pauseMenu.ApplicationPaused == true)
            canMove = false;
        else
            canMove = true;
    }
}
