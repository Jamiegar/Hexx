using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddChunkManagerSet : AddRunTimeSet<ChunkManagerSet>
{
    protected override void OnDisable()
    {
        runTimeSet.RemoveFromList(GetComponent<ChunkManager>());
    }

    protected override void OnEnable()
    {
        runTimeSet.AddToList(GetComponent<ChunkManager>());
    }
}
