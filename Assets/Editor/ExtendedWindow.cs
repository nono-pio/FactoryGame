using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExtendedWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty currentProperty;

    protected SerializedProperty selectedProperty;
    protected string selectedPropertyPath;
    protected SerializedProperty[] otherSelectedProperty;
    protected string[] otherSelectedPropertyPath;

    protected void DrawProperty(SerializedProperty property, bool drawChildren)
    {
        string lastPropPath = string.Empty;
        foreach (SerializedProperty prop in property)
        {
            if (prop.isArray && prop.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                prop.isExpanded = EditorGUILayout.Foldout(prop.isExpanded, prop.displayName);
                EditorGUILayout.EndHorizontal();

                if (prop.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProperty(prop, drawChildren);
                    EditorGUI.indentLevel--;
                }
            } else
            {
                if(!string.IsNullOrEmpty(lastPropPath) && prop.propertyPath.Contains(lastPropPath))
                    continue;
                lastPropPath = prop.propertyPath;
                EditorGUILayout.PropertyField(prop, drawChildren);
            }
        }
    }

    protected void DrawSidebar(SidebarButton[] sidebarButtons)
    {
        foreach (SidebarButton button in sidebarButtons)
        {
            SerializedProperty prop = serializedObject.FindProperty(button.name);
            if (GUILayout.Button(prop.displayName))
            {
                selectedPropertyPath = prop.propertyPath;
                otherSelectedPropertyPath = button.propertiesPath;
            }
        }

        if (!string.IsNullOrEmpty(selectedPropertyPath))
        {
            selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
            
            if(otherSelectedProperty == null || otherSelectedPropertyPath.Length != otherSelectedProperty.Length)
                otherSelectedProperty = new SerializedProperty[otherSelectedPropertyPath.Length];
            
            for (int i = 0; i < otherSelectedPropertyPath.Length; i++)
            {
                if (!string.IsNullOrEmpty(otherSelectedPropertyPath[i]))
                {
                    otherSelectedProperty[i] = serializedObject.FindProperty(otherSelectedPropertyPath[i]);
                }
            }
        }
    }

    protected void DrawField(string propertyName, bool isRelative)
    {
        if (isRelative && currentProperty != null)
        {
            EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative(propertyName), true);
        } else if (currentProperty != null)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(propertyName), true);
        }
    }

    protected void Apply()
    {
        serializedObject.ApplyModifiedProperties();
    }
}

public class SidebarButton
{
    public string name;
    public string[] propertiesPath;

    public SidebarButton(string _name, string[] _propretiesPath)
    {
        name = _name;
        propertiesPath = _propretiesPath;
    }
}
