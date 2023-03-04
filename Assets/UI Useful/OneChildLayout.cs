using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Layout/One Child Layout")]
[RequireComponent(typeof(RectTransform))]
public class OneChildLayout : LayoutGroup
{

    public enum ChildFit
    {
        Fill,
        Proportion,
        KeepRatio,
        FixedSize,
        FixedWidth,
        FixedHeight
    }

    public ChildFit childFit;
    [Range(0,1)] public float proportion;
    public Vector2 childSize;
    public Vector2 referenceCell;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        int childCount = rectChildren.Count;

        if (childCount > 1) Debug.Log("Error");
        if (childCount != 1) return;

        Vector2 rect = rectTransform.rect.size;
        Vector2 size = new Vector2(rect.x -padding.horizontal, rect.y - padding.vertical);

        bool needStop = Restriction();
        if (needStop || size.x < 0 || size.y < 0)
            return;

        childSize = GetChildSize(size);

        Vector2 restSize = size - childSize;
        SetChild(restSize);
    }

    private Vector2 GetChildSize(Vector2 parentSize)
    {
        Vector2 curChildSize = rectChildren[0].rect.size;
        switch (childFit)
        {
            case ChildFit.Fill:
                return parentSize;

            case ChildFit.Proportion:
                return parentSize * proportion;

            case ChildFit.KeepRatio:
                float ratio = Mathf.Min(parentSize.x/referenceCell.x, parentSize.y/referenceCell.y);
                return ratio * referenceCell * proportion;

            case ChildFit.FixedSize:
                return childSize;
            
            case ChildFit.FixedWidth:
                return new Vector2(childSize.x, parentSize.y * proportion);
            
            case ChildFit.FixedHeight:
                return new Vector2(parentSize.x * proportion, childSize.y);
        }
        return Vector2.one;
    }

    private void SetChild(Vector2 restSize)
    {
        Vector2 childAli = restSize * UITools.TextAnchorPosition(childAlignment);

        SetChildAlongAxis(rectChildren[0], 0, padding.left + childAli.x, childSize.x);
        SetChildAlongAxis(rectChildren[0], 1, padding.top + childAli.y, childSize.y);
    }

    private bool Restriction()
    {
        switch (childFit)
        {
            case ChildFit.Fill:
                break;

            case ChildFit.Proportion:
                if (proportion <= 0) return true;
                break;

            case ChildFit.KeepRatio:
                if (referenceCell.x <= 0 || referenceCell.y <= 0) return true;
                break;

            case ChildFit.FixedSize:
                if (childSize.x <= 0 || childSize.y <= 0) return true;
                break;
            
            case ChildFit.FixedWidth:
                if (childSize.x <= 0) return true;
                break;
            
            case ChildFit.FixedHeight:
                if (childSize.y <= 0) return true;
                break;
        }
        return false;
    }

    public override void CalculateLayoutInputVertical() {}
    public override void SetLayoutHorizontal() {}
    public override void SetLayoutVertical() {}
}