using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Layout/Pyramid Layout")]
public class PyramidLayout : LayoutGroup
{

    public float n1, b;

    public int line;
    public int[] childsDebug;

    public Vector2 childSize;

    public bool refreshEnable = true;

    public void Refresh ()
    {
        base.CalculateLayoutInputHorizontal();

        int child = rectChildren.Count;
        if (child == 0) return;

        float ratio = getLine(child);
        int[] childs = getChildLine(child, ratio);

        childsDebug = childs;

        SetChilds(childs);
    }

    private float getLine(int child)
    {
        float cur_child = n1;
        float cur_max_child = cur_child;
        int cur_line = 0;

        while (cur_max_child < child)
        {
            cur_line++;
            cur_child += b;
            cur_max_child += cur_child;
            if (cur_line > 100) return 0;
        }

        line = cur_line + 1;
        return child / cur_max_child;
    }

    private int[] getChildLine(int child, float ratio)
    {
        float[] childsFloat = new float[line];
        float cur_child = n1;
        for (int l = 0; l < line; l++)
        {
            childsFloat[l] = cur_child * ratio;
            cur_child += b;
        }

        int[] childs = new int[line];
        int sumChild = 0;
        for (int l = 0; l < line; l++)
        {
            childs[l] = Mathf.RoundToInt(childsFloat[l]);
            childsFloat[l] -= childs[l];
            sumChild += childs[l];
        }

        if (sumChild == child) return childs;
        else if (sumChild > child)
        {
            int dif = sumChild - child;
            for (int i = 0; i < dif; i++)
            {
                float min = Mathf.Min(childsFloat);
                int index = System.Array.IndexOf(childsFloat, min);
                childsFloat[index]++;
                childs[index]--;
            }
        }
        else
        {
            int dif = child - sumChild;
            for (int i = 0; i < dif; i++)
            {
                float max = Mathf.Max(childsFloat);
                int index = System.Array.IndexOf(childsFloat, max);
                childsFloat[index]--;
                childs[index]++;
            }
        }

        return childs;
    }

    private void SetChilds(int[] childs)
    {
        int childIndex = 0;
        for (int l = 0; l < line; l++)
        {
            int childLine = childs[l];
            float x_Start = (rectTransform.rect.width - childLine * childSize.x) / 2;
            for (int i = 0; i < childLine; i++)
            {
                var child = rectChildren[childIndex];
                SetChildAlongAxis(child, 0, x_Start + childSize.x * i, childSize.x);
                SetChildAlongAxis(child, 1, childSize.y * l, childSize.y);
                childIndex++;
            }
        }
    }

    public override void CalculateLayoutInputHorizontal()
    {
        if (refreshEnable)
            Refresh();
    }
    public override void CalculateLayoutInputVertical(){}
    public override void SetLayoutHorizontal(){}
    public override void SetLayoutVertical(){}
}
