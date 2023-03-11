using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OneChildLayout))]
public class OneChildLayoutEditor : Editor
{

    OneChildLayout oneChildLayout;

    bool paddingOpen = true;
    bool childOpen = true;
    bool advancedOpen = false;

    bool modeProportionD1 = false;

    public override void OnInspectorGUI()
    {
        oneChildLayout = (OneChildLayout) target;

        EditorGUI.BeginChangeCheck();
        paddingOpen = EditorGUILayout.BeginFoldoutHeaderGroup(paddingOpen, "Padding");
        if (paddingOpen)
        {
            EditorGUI.indentLevel++;

            oneChildLayout.padding.left = EditorGUILayout.IntField("Left", oneChildLayout.padding.left);
            oneChildLayout.padding.right = EditorGUILayout.IntField("Right", oneChildLayout.padding.right);
            oneChildLayout.padding.top = EditorGUILayout.IntField("Top", oneChildLayout.padding.top);
            oneChildLayout.padding.bottom = EditorGUILayout.IntField("Bottom", oneChildLayout.padding.bottom);

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        childOpen = EditorGUILayout.BeginFoldoutHeaderGroup(childOpen, "Child");
        if (childOpen)
        {
            EditorGUI.indentLevel++;

            oneChildLayout.childAlignment = (TextAnchor) EditorGUILayout.EnumPopup("Child Alignement", oneChildLayout.childAlignment);
            oneChildLayout.childFit = (OneChildLayout.ChildFit) EditorGUILayout.EnumPopup("Child Fit", oneChildLayout.childFit);

            EditorGUILayout.Space(8f);

            oneChildLayout.childSize = EditorGUILayout.Vector2Field("Child Size", oneChildLayout.childSize);

            if (oneChildLayout.childFit == OneChildLayout.ChildFit.KeepRatio)
                oneChildLayout.referenceCell = EditorGUILayout.Vector2Field("Child Reference", oneChildLayout.referenceCell);
            
            if (oneChildLayout.childFit != OneChildLayout.ChildFit.Fill && oneChildLayout.childFit != OneChildLayout.ChildFit.FixedSize)
            {
                EditorGUILayout.Space(3f);
                if (oneChildLayout.childFit == OneChildLayout.ChildFit.Proportion)
                    modeProportionD1 = EditorGUILayout.Toggle("Square Proportion", modeProportionD1);

                if (oneChildLayout.childFit == OneChildLayout.ChildFit.KeepRatio || modeProportionD1)
                {
                    float proportion = EditorGUILayout.Slider("Proportion", oneChildLayout.proportion.x, 0, 1);
                    oneChildLayout.proportion.x = proportion;
                    oneChildLayout.proportion.y = proportion;
                } else 
                {
                    if (oneChildLayout.childFit == OneChildLayout.ChildFit.Proportion ||
                        oneChildLayout.childFit == OneChildLayout.ChildFit.FixedHeight)
                        oneChildLayout.proportion.x = EditorGUILayout.Slider("Proportion X", oneChildLayout.proportion.x, 0, 1);

                    if (oneChildLayout.childFit == OneChildLayout.ChildFit.Proportion ||
                        oneChildLayout.childFit == OneChildLayout.ChildFit.FixedWidth)
                        oneChildLayout.proportion.y = EditorGUILayout.Slider("Proportion Y", oneChildLayout.proportion.y, 0, 1);
                }

            } else 
                modeProportionD1 = false;

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        advancedOpen = EditorGUILayout.BeginFoldoutHeaderGroup(advancedOpen, "Advanced");
        if (advancedOpen)
        {
            EditorGUI.indentLevel++;

            oneChildLayout.refreshEnable = EditorGUILayout.Toggle("Refresh Enable", oneChildLayout.refreshEnable);
            if (!oneChildLayout.refreshEnable)
            {
                if(GUILayout.Button("Refresh"))
                {
                    oneChildLayout.Refresh();
                }
            }

            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        bool isChange = EditorGUI.EndChangeCheck();
        if (isChange)
            oneChildLayout.CalculateLayoutInputHorizontal();
    }
}
