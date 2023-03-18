using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CircleLayout))]
public class CircleLayoutEditor : Editor
{
    CircleLayout circleLayout;

    bool spacingOpen = true;
    bool paddingOpen = true;
    bool circleOpen = true;
    bool childOpen = true;
    bool advancedOpen = false;

    private void OnEnable() {
        circleLayout = (CircleLayout) target;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        paddingOpen = EditorGUILayout.BeginFoldoutHeaderGroup(paddingOpen, "Padding");
        if (paddingOpen)
        {
            EditorGUI.indentLevel++;

            circleLayout.padding.left = EditorGUILayout.IntField("Left", circleLayout.padding.left);
            circleLayout.padding.right = EditorGUILayout.IntField("Right", circleLayout.padding.right);
            circleLayout.padding.top = EditorGUILayout.IntField("Top", circleLayout.padding.top);
            circleLayout.padding.bottom = EditorGUILayout.IntField("Bottom", circleLayout.padding.bottom);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        circleOpen = EditorGUILayout.BeginFoldoutHeaderGroup(circleOpen, "Circle");
        if (circleOpen)
        {
            EditorGUI.indentLevel++;

            circleLayout.layerFitType = (CircleLayout.LayerFitType) EditorGUILayout.EnumPopup("Layout Fit", circleLayout.layerFitType);
            if (circleLayout.layerFitType == CircleLayout.LayerFitType.Fixed)
                circleLayout.Layer = EditorGUILayout.IntField("Layer", circleLayout.Layer);
            else if (circleLayout.layerFitType == CircleLayout.LayerFitType.Custom)
            {
                circleLayout.slope = EditorGUILayout.FloatField("Slope", circleLayout.slope);
                circleLayout.intercept = EditorGUILayout.FloatField("Intercept", circleLayout.intercept);
                circleLayout.firstLayerChildMax = EditorGUILayout.FloatField("Max child at layer 1", circleLayout.firstLayerChildMax);
            }

            EditorGUILayout.Space(8f);

            circleLayout.rayonFitType = (CircleLayout.RayonFitType) EditorGUILayout.EnumPopup("Rayon Fit", circleLayout.rayonFitType);
            if (circleLayout.rayonFitType == CircleLayout.RayonFitType.Proportion)
                circleLayout.proportion = EditorGUILayout.Slider("Proportion", circleLayout.proportion, 0f, 1f);
            else if (circleLayout.rayonFitType == CircleLayout.RayonFitType.Fixed)
                circleLayout.rayon = EditorGUILayout.FloatField("Rayon", circleLayout.rayon);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        spacingOpen = EditorGUILayout.BeginFoldoutHeaderGroup(spacingOpen, "Spacing");
        if (spacingOpen)
        {
            EditorGUI.indentLevel++;

            circleLayout.spacing = EditorGUILayout.FloatField("Spacing", circleLayout.spacing);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        childOpen = EditorGUILayout.BeginFoldoutHeaderGroup(childOpen, "Child");
        if (childOpen)
        {
            EditorGUI.indentLevel++;

            circleLayout.childAlignment = (TextAnchor) EditorGUILayout.EnumPopup("Child Alignement", circleLayout.childAlignment);
            circleLayout.childFitType = (CircleLayout.ChildFitType) EditorGUILayout.EnumPopup("Child Fit", circleLayout.childFitType);
            circleLayout.useCenter = EditorGUILayout.Toggle("Can Have Center", circleLayout.useCenter);
            circleLayout.setRotation = EditorGUILayout.Toggle("Rotation", circleLayout.setRotation);

            EditorGUILayout.Space(8f);

            circleLayout.childSize = EditorGUILayout.Vector2Field("Child Size", circleLayout.childSize);

            if (circleLayout.childFitType == CircleLayout.ChildFitType.Ratio)
                circleLayout.referenceCell = EditorGUILayout.Vector2Field("Child Reference", circleLayout.referenceCell);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        advancedOpen = EditorGUILayout.BeginFoldoutHeaderGroup(advancedOpen, "Advanced");
        if (advancedOpen)
        {
            EditorGUI.indentLevel++;

            circleLayout.refreshEnable = EditorGUILayout.Toggle("Refresh Enable", circleLayout.refreshEnable);
            if (!circleLayout.refreshEnable)
            {
                if(GUILayout.Button("Refresh"))
                {
                    circleLayout.Refresh();
                }
            }

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        bool isChange = EditorGUI.EndChangeCheck();

        if(isChange)
            circleLayout.CalculateLayoutInputHorizontal();
    }
}
