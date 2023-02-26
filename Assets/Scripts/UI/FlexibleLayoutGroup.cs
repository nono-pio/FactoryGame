using UnityEngine;
using UnityEngine.UI;


public class FlexibleLayoutGroup : LayoutGroup
{

    public enum FitType
    {
        Square,
        Width,
        Height,
        FixedRows,
        FixedColumns,
        ChildSquare
    }

    public FitType fitType;

    public int rows, columns;
    public Vector2 cellSize;

    [Min(0)] public Vector2 spacing;

    public bool fitX, fitY;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        int childCount = rectChildren.Count;

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float realParentWidth = parentWidth - spacing.x * 2 - padding.left - padding.right;
        float realParentHeight = parentHeight - spacing.y * 2 - padding.top - padding.bottom;

        if (fitType == FitType.Square || fitType == FitType.Width || fitType == FitType.Height)
        {
            fitX = true;
            fitY = true;

            float sqrt = Mathf.Sqrt(childCount);
            rows = Mathf.CeilToInt(sqrt);
            columns = Mathf.CeilToInt(sqrt);
        
        } else if (fitType == FitType.ChildSquare)
        {
            fitX = true;
            fitY = true;

            //float cellLength = Mathf.Sqrt(realParentWidth * realParentHeight / transform.childCount);
            //cellSize = new Vector2(cellLength, cellLength);

            rows = Mathf.CeilToInt(Mathf.Sqrt(realParentHeight / realParentWidth * childCount));
            columns = Mathf.FloorToInt(Mathf.Sqrt(realParentWidth / realParentHeight * childCount));
        }

        if(fitType == FitType.Width || fitType == FitType.FixedColumns)
        {
            rows = Mathf.CeilToInt(childCount / (float) columns);
        }
        if (fitType == FitType.Height || fitType == FitType.FixedRows)
        {
            columns = Mathf.CeilToInt(childCount / (float) rows);
        } 

        
        float cellWidth = realParentWidth / (float) columns;
        float cellHeight = realParentHeight / (float) rows;

        if (fitType == FitType.ChildSquare)
        {
            if (cellWidth < cellHeight)
                cellSize = new Vector2(cellWidth, cellWidth);
            else 
                cellSize = new Vector2(cellHeight, cellHeight);
        } else
        {
            if (fitX)
            {
                cellSize.x = cellWidth;
            }
            if (fitY)
            {
                cellSize.y = cellHeight;
            }
        }

        int rowsCount, columnsCount;

        for (int i = 0; i < childCount; i++)
        {
            rowsCount = i / columns;
            columnsCount = i % columns;

            var item = rectChildren[i];

            var xPos = (cellSize.x * columnsCount) + (spacing.x * columnsCount) + padding.left;
            var yPos = (cellSize.y * rowsCount) + (spacing.y * rowsCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }

    }

    public override void CalculateLayoutInputVertical()
    {
        
    }

    public override void SetLayoutHorizontal()
    {
        
    }

    public override void SetLayoutVertical()
    {
        
    }
}
