using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Character<T> : MonoBehaviour, IPossessable where T : IInputActionCollection
{
    protected T m_controlledActions;
    
    [ReadOnlyInspector, SerializeField]
    protected bool m_canMove = false;

    public void Possess()
    {
        m_canMove = true;
        m_controlledActions.Enable();
    }
    public void UnPossess()
    {
        m_canMove = false;
        m_controlledActions.Disable();
    }
    protected virtual void OnEnable()
    {
        if(m_canMove == true && m_controlledActions != null)
            m_controlledActions.Enable(); 
    }
    protected virtual void OnDisable()
    {
        m_controlledActions?.Disable();
    }

}
