using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridManagerDEMO))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridManagerDEMO gridManagerDemo = (GridManagerDEMO)target;
        if (GUILayout.Button("Start testing search Algorithms"))
        {
            gridManagerDemo.TestSearchAlgorithms();
        }
    }
    
}
