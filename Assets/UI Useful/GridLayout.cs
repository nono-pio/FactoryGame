using UnityEngine;
using UnityEngine.UI;
using System;

[AddComponentMenu("Layout/Grid Layout")]
[RequireComponent(typeof(RectTransform))]
public class GridLayout : LayoutGroup
{

    public enum GridFitType
    {
        BestFit,
        FixedColumns,
        FixedRows,
        FixedColumnsRows,
        Square
    }

    public enum ChildFitType
    {
        AutoFill,
        FixedWidth,
        FixedHeight,
        FixedWidthHeight,
        Square,
        RatioCell
    }

    public enum Corner
    {
        TopLeft,
        TopRigth,
        BottomLeft,
        BottomRigth
    }

    #region SerializeField
        
    public GridFitType gridFit;
    public ChildFitType childFit;
    public Corner startCorner;

    public int rows, columns;
    public Vector2 spacing;

    [Min(10)] public Vector2 cellSize;

    [Min(0)] public Vector2 referenceCell = Vector2.one;

    #endregion

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        int childCount = rectChildren.Count;

        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        float realParentWidth = width - spacing.x - padding.horizontal;
        float realParentHeight = height - spacing.y - padding.vertical;
        Vector2 realSize = new Vector2(realParentWidth, realParentHeight);

        bool needStop = Restriction(childCount);
        if (needStop || realSize.x <= 0 || realSize.y <= 0) return;

        GetRowsColumns(realSize, childCount);

        SetCellSize(realSize);

        Vector2 restSize = GetRestWidthHeight(realSize, childCount);

        SetChild(childCount, restSize);
    }

    private void GetRowsColumns(Vector2 realSize,int childCount )
    {
        switch (gridFit)
        {
            case GridFitType.BestFit:
            {
                if (childFit == ChildFitType.FixedWidth)
                {
                    columns = Mathf.FloorToInt(realSize.x / cellSize.x);
                    rows = Mathf.CeilToInt(childCount/ (float) columns);

                } else if (childFit == ChildFitType.FixedHeight)
                {
                    rows = Mathf.FloorToInt(realSize.y / cellSize.y);
                    columns = Mathf.CeilToInt(childCount/ (float) rows);

                }else
                {
                    float floatColumns = Mathf.Sqrt(realSize.x / realSize.y * referenceCell.y/referenceCell.x * childCount);
                    columns = Mathf.FloorToInt(floatColumns);

                    float floatRows = Mathf.Sqrt(realSize.y / realSize.x * referenceCell.x/referenceCell.y * childCount);
                    rows = Mathf.FloorToInt(floatRows);

                    float[] childArea = new float[4];
                    int[] curRows = {rows, rows + 1, rows, rows + 1};
                    int[] curColumns = {columns, columns, columns + 1, columns + 1};

                    for(int i = 0; i < 4; i++)
                    {
                        if (curRows[i] * curColumns[i] >= childCount)
                        {
                            Vector2 child = CalculateChildSize(realSize, curColumns[i], curRows[i]);
                            childArea[i] = child.x * child.y;
                        }
                    }
                    
                    int indexMaxSize = Array.IndexOf(childArea, Mathf.Max(childArea));

                    if (indexMaxSize == -1 || Mathf.Max(childArea) == 0) Debug.Log("Error");
                    else 
                    {
                        columns = curColumns[indexMaxSize];
                        rows = curRows[indexMaxSize];
                    }
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

    private Vector2 CalculateChildSize(Vector2 realSize, int _columns, int _rows)
    {
        float cellWidth = realSize.x/ (float) _columns;
        float cellHeight = realSize.y / (float) _rows;
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
                float sideLenght = Mathf.Min(cellWidth, cellHeight);

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

    private void SetCellSize(Vector2 realSize)
    {
        cellSize = CalculateChildSize(realSize, columns, rows);
    }

    private Vector2 GetRestWidthHeight(Vector2 realSize, int childCount)
    {
        float restWidth = realSize.x - cellSize.x * columns;
        float restHeight = realSize.y - cellSize.y * rows;
        return new Vector2(restWidth, restHeight);
    }

    private void SetChild(int childCount, Vector2 restSize)
    {
        Vector2 childAli = restSize * UITools.TextAnchorPosition(childAlignment);

        bool startTop = startCorner == Corner.TopLeft || startCorner == Corner.TopRigth;
        bool startLeft = startCorner == Corner.BottomLeft || startCorner == Corner.TopLeft;

        int rowsCount, columnsCount;

        float spacingX = spacing.x / (float) (columns - 1);
        float spacingY = spacing.y / (float) (rows - 1);

        for (int i = 0; i < childCount; i++)
        {
            rowsCount = startTop ? i / columns : (rows - 1) - i / columns;
            columnsCount = startLeft ? i % columns : (columns - 1) - i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x + spacingX) * columnsCount + padding.left + childAli.x;
            var yPos = (cellSize.y + spacingY) * rowsCount + padding.top + childAli.y;

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
                if(childFit == ChildFitType.AutoFill) childFit = ChildFitType.Square;
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
                referenceCell = cellSize;
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