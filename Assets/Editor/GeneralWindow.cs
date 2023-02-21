using UnityEngine;
using UnityEditor;

public class GeneralWindow : EditorWindow
{
    [MenuItem("Window/Game Settings")]
    public static void ShowWindow()
    {
        GetWindow<GeneralWindow>("General Settings");
    }

    private void OnGUI()
    {
        
    }
}
