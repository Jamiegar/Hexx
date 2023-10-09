using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGridMangerSet : AddRunTimeSet<GridManagerSet>
{
    protected override void OnDisable()
    {
        runTimeSet.RemoveFromList(GetComponent<GridManager>());
    }

    protected override void OnEnable()
    {
        runTimeSet.AddToList(GetComponent<GridManager>());
    }
}
