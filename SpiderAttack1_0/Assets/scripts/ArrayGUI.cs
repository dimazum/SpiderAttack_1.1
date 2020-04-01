using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SceneGenerator))]
public class ArrayGUI : Editor
{
    SceneGenerator sceneGenerator;

    public ArrayGUI()
    {
        sceneGenerator = new SceneGenerator();
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Instantiate"))
        {
            sceneGenerator.BuildScene();
        }
    }

}
