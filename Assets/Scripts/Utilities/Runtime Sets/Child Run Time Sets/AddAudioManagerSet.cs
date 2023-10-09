using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAudioManagerSet : AddRunTimeSet<AudioManagerSet>
{
    protected override void OnDisable()
    {
        runTimeSet.RemoveFromList(GetComponent<AudioManager>());
    }

    protected override void OnEnable()
    {
        runTimeSet.AddToList(GetComponent<AudioManager>());
    }
}
