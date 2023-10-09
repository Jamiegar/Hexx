using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(GridManager))]
public class CE_MapManager : Editor
{
    public override void OnInspectorGUI()
    {
        GridManager gridManager = (GridManager)target;
        if (gridManager == null)
            return;

        if(DrawDefaultInspector())
        {
            if (gridManager.AutoUpdate)
            {
                gridManager.ClearGrid();
                gridManager.Init();
            }
        }


        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Generate Grid"))
        {
            gridManager.Init();
        }

        if(GUILayout.Button("Clear Grid"))
        {
            gridManager.ClearGrid();
        }
        GUILayout.EndHorizontal();
    }


}
