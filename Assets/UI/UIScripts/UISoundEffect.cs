using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UISoundEffect : MonoBehaviour, IInitalizable
{
    [SerializeField] private Button[] _buttons;
    [SerializeField] private Sound _sfxOnClick;

    [Header("Run Time Set"), SerializeField]
    private AudioManagerSet _audioManagerSet;
    private AudioManager _audioManager;

    public void Init()
    {
        _audioManager = _audioManagerSet.GetItemIndex(0);
        
        foreach (Button b in _buttons)
        {
            b.onClick.AddListener(OnClicked);
        }    
    }

    private void OnClicked()
    {
        _audioManager.Play(_sfxOnClick);
    }

}
