using UnityEngine;
using UnityEngine.UI;
using System;

[AddComponentMenu("Layout/List Layout")]
[RequireComponent(typeof(RectTransform))]
public class ListLayout : LayoutGroup
{

    public enum ChildFitType
    {
        AutoFill,
        FixedWidth,
        FixedHeight,
        FixedWidthHeight,
        RatioCell
    }

    #region SerializeField
        
    public ChildFitType childFit; //editor
    public bool isVertical;
    public bool reverseOrder;

    public float spacing; //editor
    public bool spacingFitListSize; //editor

    public Vector2 cellSize;//editor
    public Vector2 referenceCell = Vector2.one;//editor

    public bool refreshEnable = true;//editor

    public ChildUse childUse;
    public int fixedChild;

    #endregion

    public override void CalculateLayoutInputHorizontal()
    {
        if (refreshEnable)
            Refresh();
    }

    public void Refresh()
    {
        base.CalculateLayoutInputHorizontal();

        int childCount = GetChildCount();

        bool needStop = Restriction(childCount);
        if (needStop) return;

        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        float realParentHeight;
        float realParentWidth;
        if (isVertical)
        {
            if (spacingFitListSize)
                realParentHeight = height * (1 - spacing) - padding.vertical;
            else
                realParentHeight = height - spacing - padding.vertical;

            realParentWidth = width - padding.horizontal;
        } else
        {
            if (spacingFitListSize)
                realParentWidth = width * (1 - spacing) - padding.horizontal;
            else
                realParentWidth = width - spacing - padding.horizontal;
                
            realParentHeight = height - padding.vertical;
        }

        Vector2 realSize = new Vector2(realParentWidth, realParentHeight);
        if (realSize.x <= 0 || realSize.y <= 0) return;

        SetCellSize(realSize, childCount);

        Vector2 restSize = GetRestWidthHeight(realSize, childCount);

        SetChild(childCount, restSize);
    }

    private int GetChildCount()
    {
        switch (childUse)
        {
            case ChildUse.ActiveChild : return rectChildren.Count;
            case ChildUse.AllChildren : return transform.childCount;
            case ChildUse.FixedChild : return fixedChild;
        }
        return 1;
    }

    private Vector2 CalculateChildSize(Vector2 realSize, int childCount)
    {
        float cellWidth;
        float cellHeight;
        if (isVertical)
        {
            cellWidth = realSize.x;
            cellHeight = realSize.y / (float) childCount;
        } else 
        {
            cellWidth = realSize.x / (float) childCount;
            cellHeight = realSize.y;
        }
        
        switch (childFit)
        {
            case ChildFitType.AutoFill:
            {
                return new Vector2(cellWidth, cellHeight);
            }
            case ChildFitType.FixedWidth:
            {
                return new Vector2(cellSize.x, cellHeight);
            }
            case ChildFitType.FixedHeight:
            {
                return new Vector2(cellWidth, cellSize.y);
            }
            case ChildFitType.FixedWidthHeight:
            {
                return cellSize;
            }
            case ChildFitType.RatioCell:
            {
                float ratio = Mathf.Min(cellWidth/referenceCell.x, cellHeight/referenceCell.y);
                return new Vector2(ratio * referenceCell.x, ratio * referenceCell.y);
            }
        }
        return Vector2.zero;
    }

    private void SetCellSize(Vector2 realSize, int childCount)
    {
        cellSize = CalculateChildSize(realSize, childCount);
    }

    private Vector2 GetRestWidthHeight(Vector2 realSize, int childCount)
    {
        float restWidth;
        float restHeight;

        if (isVertical)
        {
            restHeight = realSize.y - cellSize.y * childCount;
            restWidth = realSize.x - cellSize.x;
        } else
        {
            restHeight = realSize.y - cellSize.y;
            restWidth = realSize.x - cellSize.x * childCount;
        }


        return new Vector2(restWidth, restHeight);
    }

    private void SetChild(int childCount, Vector2 restSize)
    {
        Vector2 childAli = restSize * UITools.TextAnchorPosition(childAlignment);

        float spacing1;
        if (childCount > 1)
            spacing1 = (spacingFitListSize ? (isVertical ? rectTransform.rect.height : rectTransform.rect.width) * spacing : spacing) / (float) (childCount - 1);
        else spacing1 = 0;


        int index;
        for (int i = 0; i < transform.childCount; i++)
        {
            index = reverseOrder ? childCount - 1 - i : i;

            if (index >= transform.childCount || index < 0) continue;
            var item = transform.GetChild(index).GetComponent<RectTransform>();
            if (item == null) continue;

            float xPos;
            float yPos;
            if (isVertical)
            {
                xPos = padding.left + childAli.x;
                yPos = (cellSize.y + spacing1) * i + padding.top + childAli.y;

            } else 
            {
                yPos = padding.top + childAli.y;
                xPos = (cellSize.x + spacing1) * i + padding.left + childAli.x;
            }

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
    }

    private bool Restriction(int childCount)
    {
        bool stop = false;
        if (childCount == 0) return true;
        switch (childFit)
        {
            case ChildFitType.AutoFill:
            {
                referenceCell = Vector2.one;
                break;
            }
            case ChildFitType.FixedWidth:
            {
                if (cellSize.x == 0) stop = true;
                break;
            }
            case ChildFitType.FixedHeight:
            {
                if (cellSize.y == 0) stop = true;
                break;
            }
            case ChildFitType.FixedWidthHeight:
            {
                if (cellSize.x == 0 || cellSize.y == 0) stop = true;
                referenceCell = cellSize;
                break;
            }
            case ChildFitType.RatioCell:
            {
                if(referenceCell.x == 0 || referenceCell.y == 0) stop = true;
                break;
            }
        }
        return stop;
    }

    public override void CalculateLayoutInputVertical() {}

    public override void SetLayoutHorizontal() {}

    public override void SetLayoutVertical() {}
}