using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(ListLayout))]
public class ListLayoutEditor : Editor
{

    ListLayout listLayout;

    bool spacingOpen = true;
    bool paddingOpen = true;
    bool childOpen = true;
    bool listOpen = true;
    bool advancedOpen = false;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        listLayout = (ListLayout) target;

        EditorGUI.BeginChangeCheck();
        paddingOpen = EditorGUILayout.BeginFoldoutHeaderGroup(paddingOpen, "Padding");
        if (paddingOpen)
        {
            EditorGUI.indentLevel++;

            listLayout.padding.left = EditorGUILayout.IntField("Left", listLayout.padding.left);
            listLayout.padding.right = EditorGUILayout.IntField("Right", listLayout.padding.right);
            listLayout.padding.top = EditorGUILayout.IntField("Top", listLayout.padding.top);
            listLayout.padding.bottom = EditorGUILayout.IntField("Bottom", listLayout.padding.bottom);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        spacingOpen = EditorGUILayout.BeginFoldoutHeaderGroup(spacingOpen, "Spacing");
        if (spacingOpen)
        {
            EditorGUI.indentLevel++;

            listLayout.spacingFitListSize = EditorGUILayout.Toggle("Spacing Auto Fit", listLayout.spacingFitListSize);
            if (listLayout.spacingFitListSize)
            {
                listLayout.spacing = EditorGUILayout.Slider("Horizontale proportion", listLayout.spacing, 0f, 1f);
            } else 
            {
                listLayout.spacing = EditorGUILayout.FloatField("Spacing", listLayout.spacing);
            }

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        childOpen = EditorGUILayout.BeginFoldoutHeaderGroup(childOpen, "Child");
        if (childOpen)
        {
            EditorGUI.indentLevel++;

            listLayout.childAlignment = (TextAnchor) EditorGUILayout.EnumPopup("Child Alignement", listLayout.childAlignment);
            listLayout.childFit = (ListLayout.ChildFitType) EditorGUILayout.EnumPopup("Child Fit", listLayout.childFit);

            EditorGUILayout.Space(8f);

            listLayout.cellSize = EditorGUILayout.Vector2Field("Cell Size", listLayout.cellSize);

            if (listLayout.childFit == ListLayout.ChildFitType.RatioCell)
                listLayout.referenceCell = EditorGUILayout.Vector2Field("Cell Reference", listLayout.referenceCell);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        listOpen = EditorGUILayout.BeginFoldoutHeaderGroup(listOpen, "Grid");
        if (listOpen)
        {
            EditorGUI.indentLevel++;

            listLayout.reverseOrder = EditorGUILayout.Toggle("Reverse Order", listLayout.reverseOrder);

            VH curVH = listLayout.isVertical ? VH.Vertical : VH.Horizontal;
            VH vh = (VH) EditorGUILayout.EnumPopup("Axis", curVH);
            listLayout.isVertical = vh == VH.Vertical;

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        advancedOpen = EditorGUILayout.BeginFoldoutHeaderGroup(advancedOpen, "Advanced");
        if (advancedOpen)
        {
            EditorGUI.indentLevel++;

            listLayout.refreshEnable = EditorGUILayout.Toggle("Refresh Enable", listLayout.refreshEnable);
            if (!listLayout.refreshEnable)
            {
                if(GUILayout.Button("Refresh"))
                {
                    listLayout.Refresh();
                }
            }

            listLayout.childUse = (ChildUse)EditorGUILayout.EnumPopup("Child use", listLayout.childUse);
            if (listLayout.childUse == ChildUse.FixedChild)
            {
                listLayout.fixedChild = EditorGUILayout.IntField("Fixed Child", listLayout.fixedChild);
                if (listLayout.fixedChild > 500) listLayout.fixedChild = 500;
                else if (listLayout.fixedChild < 1)listLayout.fixedChild = 1; 
            }

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        bool isChange = EditorGUI.EndChangeCheck();
        if (isChange)
            listLayout.CalculateLayoutInputHorizontal();
    }
}

enum VH
{
    Vertical,
    Horizontal
}