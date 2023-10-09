using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class CE_NoiseVisualizer : EditorWindow
{
    [MenuItem("Tools/Noise Visualizer")]
    private static void CreateWindow()
    {
        GetWindow<CE_NoiseVisualizer>("Noise Visualizer");
    }


}
