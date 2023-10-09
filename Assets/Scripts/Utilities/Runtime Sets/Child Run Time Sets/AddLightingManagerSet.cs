using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLightingManagerSet : AddRunTimeSet<LightingManagerSet>
{
    protected override void OnDisable()
    {
        runTimeSet.RemoveFromList(GetComponent<LightingManager>());
    }

    protected override void OnEnable()
    {
        runTimeSet.AddToList(GetComponent<LightingManager>());
    }


}
