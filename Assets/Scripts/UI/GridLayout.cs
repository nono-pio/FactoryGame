using UnityEngine;
using UnityEngine.UI;
using System;

public class GridLayout : LayoutGroup
{

    enum GridFitType
    {
        BestFit,
        FixedColumns,
        FixedRows,
        FixedColumnsRows,
        Square
    }

    enum ChildFitType
    {
        AutoFill,
        FixedWidth,
        FixedHeight,
        FixedWidthHeight,
        Square,
        RatioCell
    }

    [SerializeField] private GridFitType gridFit;
    [SerializeField] private ChildFitType childFit;

    [SerializeField] private int rows, columns;
    [SerializeField] private Vector2 spacing;

    [SerializeField] private Vector2 cellSize;

    [SerializeField] private Vector2 referenceCell = Vector2.one;

    private float childSizeModeSquare;

    private float restWidth;
    private float restHeight;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        int childCount = rectChildren.Count;

        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        float realParentWidth = width - spacing.x - padding.left - padding.right;
        float realParentHeight = height - spacing.y - padding.top - padding.bottom;

        bool needStop = Restriction(childCount);
        if (needStop || realParentWidth <= 0 || realParentHeight <= 0) return;

        GetRowsColumns(realParentWidth, realParentHeight, childCount);

        SetCellSize(realParentWidth, realParentHeight);

        GetRestWidthHeight(realParentWidth, realParentHeight, childCount);

        SetChild(childCount);
    }

    private void GetRowsColumns(float realParentWidth,float realParentHeight,int childCount )
    {
        switch (gridFit)
        {
            case GridFitType.BestFit:
            {
                float floatColumns = Mathf.Sqrt(realParentWidth/realParentHeight * referenceCell.y/referenceCell.x * childCount);
                columns = Mathf.FloorToInt(floatColumns);

                float floatRows = Mathf.Sqrt(realParentHeight/realParentWidth * referenceCell.x/referenceCell.y * childCount);
                rows = Mathf.FloorToInt(floatRows);

                float[] childArea = new float[4];
                int[] curRows = {rows, rows + 1, rows, rows + 1};
                int[] curColumns = {columns, columns, columns + 1, columns + 1};

                for(int i = 0; i < 4; i++)
                {
                    if (curRows[i] * curColumns[i] >= childCount)
                    {
                        Vector2 child = CalculateChildSize(realParentWidth, realParentHeight, curColumns[i], curRows[i]);
                        childArea[i] = child.x * child.y;
                    }
                }
                
                int indexMaxSize = Array.IndexOf(childArea, Mathf.Max(childArea));

                if (indexMaxSize == -1 || Mathf.Max(childArea) == 0) Debug.Log("Error");
                else 
                {
                    childSizeModeSquare = childArea[indexMaxSize];
                    columns = curColumns[indexMaxSize];
                    rows = curRows[indexMaxSize];
                }
                break;
            }
            case GridFitType.FixedColumns:
            {
                rows = Mathf.CeilToInt(childCount /(float) columns);
                break;
            }
            case GridFitType.FixedRows:
            {
                columns = Mathf.CeilToInt(childCount / (float) rows);
                break;
            }
            case GridFitType.FixedColumnsRows:
            {
                break;
            }
            case GridFitType.Square:
            {
                float sqrt = Mathf.Sqrt(childCount);
                rows = Mathf.CeilToInt(sqrt);
                columns = Mathf.CeilToInt(sqrt);
                break;
            }
        }
    }

    private Vector2 CalculateChildSize(float realParentWidth, float realParentHeight, int _columns, int _rows)
    {
        float cellWidth = realParentWidth/ (float) _columns;
        float cellHeight = realParentHeight / (float) _rows;
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
            case ChildFitType.Square:
            {
                float sideLenght;
                if (cellHeight > cellWidth)
                    sideLenght = cellWidth;
                else 
                    sideLenght = cellHeight;

                return new Vector2(sideLenght, sideLenght);
            }
            case ChildFitType.RatioCell:
            {
                float ratio = Mathf.Min(cellWidth/referenceCell.x, cellHeight/referenceCell.y);
                return new Vector2(ratio * referenceCell.x, ratio * referenceCell.y);
            }
        }
        return Vector2.zero;
    }

    private void SetCellSize(float realParentWidth, float realParentHeight)
    {
        cellSize = CalculateChildSize(realParentWidth, realParentHeight, columns, rows);
    }

    private void GetRestWidthHeight(float realParentWidth, float realParentHeight, int childCount)
    {
        restWidth = realParentWidth - cellSize.x * columns;
        restHeight = realParentHeight - cellSize.y * rows;
    }

    private void SetChild(int childCount)
    {
        float childAliX = 0, childAliY = 0;
        switch (childAlignment)
        {
            case TextAnchor.UpperLeft:
                break;
            case TextAnchor.UpperCenter:
                childAliX = restWidth / 2;
                break;
            case TextAnchor.UpperRight:
                childAliX = restWidth;
                break;
            case TextAnchor.MiddleLeft:
                childAliY = restHeight / 2;
                break;
            case TextAnchor.MiddleCenter:
                childAliX = restWidth / 2;
                childAliY = restHeight / 2;
                break;
            case TextAnchor.MiddleRight:
                childAliX = restWidth;
                childAliY = restHeight / 2;
                break;
            case TextAnchor.LowerLeft:
                childAliY = restHeight;
                break;
            case TextAnchor.LowerCenter:
                childAliX = restWidth / 2;
                childAliY = restHeight;
                break;
            case TextAnchor.LowerRight:
                childAliX = restWidth;
                childAliY = restHeight;
                break;
        }

        int rowsCount, columnsCount;

        float spacingX = spacing.x / (float) (columns - 1);
        float spacingY = spacing.y / (float) (rows - 1);

        for (int i = 0; i < childCount; i++)
        {
            rowsCount = i / columns;
            columnsCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x + spacingX) * columnsCount + padding.left + childAliX;
            var yPos = (cellSize.y + spacingY) * rowsCount + padding.top + childAliY;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
    }

    private bool Restriction(int childCount)
    {
        bool stop = false;
        if (childCount == 0) return true;
        switch (gridFit)
        {
            case GridFitType.BestFit:
            {
                if (childFit != ChildFitType.Square && childFit != ChildFitType.RatioCell) childFit = ChildFitType.Square;
                break;
            }
            case GridFitType.FixedColumns:
            {
                if (columns == 0) stop = true;
                break;
            }
            case GridFitType.FixedRows:
            {
                if (rows == 0) stop = true;
                break;
            }
            case GridFitType.FixedColumnsRows:
            {
                if (columns == 0 || rows == 0) stop = true;
                break;
            }
            case GridFitType.Square:
            {
                break;
            }
        }
        switch (childFit)
        {
            case ChildFitType.AutoFill:
            {
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
                break;
            }
            case ChildFitType.Square:
            {
                referenceCell = Vector2.one;
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
