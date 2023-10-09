using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour, IInitalizable
{
    [SerializeField] private Canvas[] _allCanvases;

    public PauseMenu PauseMenu { get; private set; } 
    public void Init()
    {
        foreach (Canvas canvas in _allCanvases) //Initalises all the UI within the canvas
        {
            PauseMenu = canvas.GetComponent<PauseMenu>(); //Gets refrence to pasue menu

            IInitalizable[] initObj = canvas.GetComponentsInChildren<IInitalizable>();

            foreach (IInitalizable obj in initObj)
            {
                obj.Init();
            }
        }
    }
}
