using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AssetHandler
{
    [OnOpenAsset()]
    public static bool OpenWindow(int instanceId, int line)
    {
        GameManager dataObject = EditorUtility.InstanceIDToObject(instanceId) as GameManager;
        if (dataObject != null)
        {
            GeneralWindow.Open(dataObject);
            return true;
        }
        return false;
    }
}

[CustomEditor(typeof(GameManager))]
public class GameManagerDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Window"))
        {
            GeneralWindow.Open((GameManager)target);
        }
    }
}
