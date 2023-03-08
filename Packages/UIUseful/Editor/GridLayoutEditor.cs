using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(GridLayout))]
public class GridLayoutEditor : Editor
{

    GridLayout gridLayout;

    bool spacingOpen = true;
    bool paddingOpen = true;
    bool childOpen = true;
    bool gridOpen = true;
    bool advancedOpen = false;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        gridLayout = (GridLayout) target;

        paddingOpen = EditorGUILayout.BeginFoldoutHeaderGroup(paddingOpen, "Padding");
        if (paddingOpen)
        {
            EditorGUI.indentLevel++;

            gridLayout.padding.left = EditorGUILayout.IntField("Left", gridLayout.padding.left);
            gridLayout.padding.right = EditorGUILayout.IntField("Right", gridLayout.padding.right);
            gridLayout.padding.top = EditorGUILayout.IntField("Top", gridLayout.padding.top);
            gridLayout.padding.bottom = EditorGUILayout.IntField("Bottom", gridLayout.padding.bottom);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        spacingOpen = EditorGUILayout.BeginFoldoutHeaderGroup(spacingOpen, "Spacing");
        if (spacingOpen)
        {
            EditorGUI.indentLevel++;

            gridLayout.spacingFitGridSize = EditorGUILayout.Toggle("Spacing Auto Fit", gridLayout.spacingFitGridSize);
            if (gridLayout.spacingFitGridSize)
            {
                gridLayout.spacing.x = EditorGUILayout.Slider("Horizontale proportion", gridLayout.spacing.x, 0f, 1f);
                gridLayout.spacing.y = EditorGUILayout.Slider("Verticale proportion", gridLayout.spacing.y, 0f, 1f);
            } else 
            {
                gridLayout.spacing = EditorGUILayout.Vector2Field("Spacing", gridLayout.spacing);
            }

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        childOpen = EditorGUILayout.BeginFoldoutHeaderGroup(childOpen, "Child");
        if (childOpen)
        {
            EditorGUI.indentLevel++;

            gridLayout.childAlignment = (TextAnchor) EditorGUILayout.EnumPopup("Child Alignement", gridLayout.childAlignment);
            gridLayout.startCorner = (GridLayout.Corner) EditorGUILayout.EnumPopup("Start Corner", gridLayout.startCorner);
            gridLayout.childFit = (GridLayout.ChildFitType) EditorGUILayout.EnumPopup("Child Fit", gridLayout.childFit);

            EditorGUILayout.Space(8f);

            gridLayout.cellSize = EditorGUILayout.Vector2Field("Cell Size", gridLayout.cellSize);

            if (gridLayout.childFit == GridLayout.ChildFitType.RatioCell)
                gridLayout.referenceCell = EditorGUILayout.Vector2Field("Cell Reference", gridLayout.referenceCell);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        gridOpen = EditorGUILayout.BeginFoldoutHeaderGroup(gridOpen, "Grid");
        if (gridOpen)
        {
            EditorGUI.indentLevel++;

            gridLayout.gridFit = (GridLayout.GridFitType) EditorGUILayout.EnumPopup("Grid Fit", gridLayout.gridFit);
            gridLayout.columns = EditorGUILayout.IntField("Columns", gridLayout.columns);
            gridLayout.rows = EditorGUILayout.IntField("Rows", gridLayout.rows);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        advancedOpen = EditorGUILayout.BeginFoldoutHeaderGroup(advancedOpen, "Advanced");
        if (advancedOpen)
        {
            EditorGUI.indentLevel++;

            gridLayout.refreshEnable = EditorGUILayout.Toggle("Refresh Enable", gridLayout.refreshEnable);
            if (!gridLayout.refreshEnable)
            {
                if(GUILayout.Button("Refresh"))
                {
                    gridLayout.Refresh();
                }
            }

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        gridLayout.CalculateLayoutInputHorizontal();
    }
}
