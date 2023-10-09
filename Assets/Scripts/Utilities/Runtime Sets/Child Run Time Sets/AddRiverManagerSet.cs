using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRiverManagerSet : AddRunTimeSet<RiverManagerSet>
{
    protected override void OnDisable()
    {
        runTimeSet.RemoveFromList(GetComponent<RiverManager>());
    }

    protected override void OnEnable()
    {
        runTimeSet.AddToList(GetComponent<RiverManager>());
    }
}
