using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelOpener : MonoBehaviour, IInitalizable
{
    [SerializeField] private GameObject Panel;

    private Animator animator;
    public void Init()
    {
        animator = Panel.GetComponent<Animator>();
    }

    public void TogglePanel()
    {
        if (animator == null || Panel == null)
            return;

        bool isOpen = animator.GetBool("open");

        animator.SetBool("open", !isOpen);
    }


}
