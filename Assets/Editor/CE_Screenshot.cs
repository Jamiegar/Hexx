using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScreenshotHandeler))]
public class CE_Screenshot : Editor
{
    ScreenshotHandeler handeler;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        if(GUILayout.Button("Take Screenshot"))
        {
            handeler = (ScreenshotHandeler)target;

            if (handeler != null)
                handeler.TakeScreenShot();
        }

    }


}
