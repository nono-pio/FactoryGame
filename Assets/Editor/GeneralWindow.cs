using UnityEngine;
using UnityEditor;
using System;

public class GeneralWindow : ExtendedWindow
{

    private SidebarButton[] sidebarButtons = {
        new SidebarButton("items", new string[] {}),
        new SidebarButton("crafts", new string[] {"defaultCraft"})
    };
    
    public static void Open(GameManager dataGame)
    {
        GeneralWindow window = GetWindow<GeneralWindow>("General Settings");
        window.serializedObject = new SerializedObject(dataGame);
    }

    private void OnGUI()
    {
        currentProperty = serializedObject.FindProperty("items");
        
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

        DrawSidebar(sidebarButtons);

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

        if(selectedProperty != null)
        {
            EditorGUILayout.PropertyField(selectedProperty, true);
            if(otherSelectedProperty != null)
            {
                foreach (var prop in otherSelectedProperty)
                {
                    if (prop != null)
                        EditorGUILayout.PropertyField(prop);
                }
            }
        } else
        {
            EditorGUILayout.LabelField("Select an item from the list");
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        Apply();
    }

    void DrawSelectedPropertiesPanel()
    {
        currentProperty = selectedProperty;

        EditorGUILayout.BeginHorizontal();

        //DrawField("", true);

        EditorGUILayout.EndHorizontal();
    }
}
