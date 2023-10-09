using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridTransform))]
public class CharacterSound : MonoBehaviour, IInitalizable
{
    private Sound lastSound;


    [Header("Run Time Sets")]
    [SerializeField] 
    private AudioManagerSet m_audioManagerSet;
    private AudioManager m_audioManager;
    
    public void Init()
    {
        m_audioManager = m_audioManagerSet.GetItemIndex(0);
    }

    public void OnPositionChanage(TileType newTile) //When Player position changed this function is called
    {
        if (newTile.m_tile.AudioOnTile == null)
            return;
        
        if (lastSound != null)
        {
            if (lastSound.name == newTile.m_tile.AudioOnTile.name)
                return;

            
            m_audioManager.FadeInOutSound(newTile.m_tile.AudioOnTile, lastSound);
            lastSound = newTile.m_tile.AudioOnTile;
            Debug.Log("Play Sound: " + newTile.m_tile.AudioOnTile);
            return;
        }

        Debug.Log("Play Sound: " + newTile.m_tile.AudioOnTile);
        m_audioManager.FadeSoundIn(newTile.m_tile.AudioOnTile);
        lastSound = newTile.m_tile.AudioOnTile;
    }


}
