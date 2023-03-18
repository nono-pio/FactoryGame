using UnityEngine;
using UnityEngine.UI;

public class CircleLayout : LayoutGroup
{

    public enum LayerFitType
    {
        Auto,
        Fixed,
        Custom
    }

    public enum RayonFitType
    {
        Auto,
        Proportion,
        Fixed
    }

    public enum ChildFitType
    {
        FixedSize,
        FixedWidth,
        FixedHeight,
        Square,
        Circle,
        Ratio
    }

    public int Layer; //editor
    public LayerFitType layerFitType; //editor
    public float slope, intercept, firstLayerChildMax;

    public bool useCenter; //editor
    public bool isCenter = false;

    public float rayon; //editor
    public RayonFitType rayonFitType; //editor
    [Range(0, 1)] public float proportion; //editor

    public ChildFitType childFitType; //editor
    public Vector2 childSize; //editor
    public Vector2 referenceCell; //editor

    public float spacing; //editor

    public bool setRotation; //editor

    public bool refreshEnable = true; //editor

    public void Refresh()
    {
        base.CalculateLayoutInputHorizontal();

        int child = rectChildren.Count;

        if(Restriction(child)) return;

        Vector2 realSizeParent = rectTransform.rect.size - childSize;

        if (rayonFitType != RayonFitType.Fixed)
            rayon = (Mathf.Min(realSizeParent.x, realSizeParent.y) / 2 - padding.left) * proportion;

        getLayer(child);
        int[] childPerLayer = getChildPerLayer(child);
        foreach (var c in childPerLayer)
        {
            //Debug.Log(c);
        }

        if(childPerLayer == null) return;

        Vector3[] position = getPos(child, childPerLayer);
        Quaternion[] rotation = null;
        if (setRotation) rotation = getRotation(child, childPerLayer);

        Vector2 restSize = getRest(realSizeParent);

        int indexLayer = 0, childInLayer = 0;
        Vector2 curChildSizeLayer = getChildSize(childPerLayer[indexLayer], indexLayer);
        bool setSizeCenter = isCenter ? false : true;
        for (int i = 0; i < child; i++)
        {

            if (!setSizeCenter)
            {
                setSizeCenter = true;
                childInLayer--;
            }else if (childInLayer > childPerLayer[indexLayer])
            {
                indexLayer++;
                childInLayer = 1;
                curChildSizeLayer = getChildSize(childPerLayer[indexLayer], indexLayer);
            }

            var item = rectChildren[i];
            if (setRotation) item.rotation = rotation[i];
            else item.rotation = new Quaternion();

            SetChildAlongAxis(item, 0, position[i].x + rayon + restSize.x, curChildSizeLayer.x);
            SetChildAlongAxis(item, 1, position[i].y + rayon + restSize.y, curChildSizeLayer.y);

            childInLayer++;
        }
    }

    public Vector2 getRest(Vector2 realSizeParent)
    {
        return (realSizeParent - 2 * new Vector2(rayon, rayon)) *  UITools.TextAnchorPosition(childAlignment);
    }

    public void getLayer(int child)
    {
        switch (layerFitType)
        {
            case LayerFitType.Auto:
                Layer = Mathf.CeilToInt((Mathf.Sqrt(1 + 1.33333333f * child) - 1) / 2);
                break;
            case LayerFitType.Fixed:
                break;
            case LayerFitType.Custom:
                Vector2[] root = UITools.thirdDegreeRoot(-slope/6, intercept/2, slope/6 - intercept/2 - firstLayerChildMax, child);
                foreach (var r in root)
                    if (r.y == 0)
                    {
                        Layer = Mathf.CeilToInt(r.x);
                        break;
                    }
                break;
        }
    }

    public int[] getChildPerLayer(int child)
    {

        if(useCenter)
        {
            child--;
            if (layerFitType == LayerFitType.Custom)
            {
                isCenter = Mathf.RoundToInt(child/ (float) Layer - slope/6 *(Layer*Layer -1) + intercept/2 * (Layer - 1)) >= firstLayerChildMax;
                if (!isCenter) child++;
            } else 
            {
                if (Mathf.RoundToInt(2 * (child / (float)(Layer * (Layer + 1)))) >= 6)
                {
                    isCenter = true;
                }
                else
                {
                    isCenter = false;
                    child++;
                } 
            }
        } else
            isCenter = false;

        float[] childPerLayer = new float[Layer];
        childPerLayer[0] = (layerFitType == LayerFitType.Custom)? child/ (float) Layer - slope/6 *(Layer*Layer -1) + intercept/2 * (Layer - 1) : 2 * (child / (float)(Layer * (Layer + 1)));
        //Debug.Log(childPerLayer[0]);

        float ratio = childPerLayer[0]/firstLayerChildMax;
        for (int k = 1; k < Layer; k++)
        {
            childPerLayer[k] = childPerLayer[k - 1] + (slope * k + intercept) * ratio;
            //Debug.Log(childPerLayer[k]);
        }

        int[] new_childPerLayer = new int[Layer];
        for (int i = 0; i < Layer; i++)
        {
            new_childPerLayer[i] = Mathf.RoundToInt(childPerLayer[i]);
            if (new_childPerLayer[i] <= 0) return null;
            childPerLayer[i] -= new_childPerLayer[i];
        }

        int sumChild = 0;
        foreach (int s in new_childPerLayer)
            sumChild += s;

        if (sumChild != child)
        {
            int dif = child - sumChild;
            if (dif > 0)
            {
                for (int i = 0; i < dif; i++)
                {
                    int index = System.Array.IndexOf(childPerLayer, Mathf.Max(childPerLayer));
                    new_childPerLayer[index]++;
                    childPerLayer[index]--;
                }
            }
            else
            {
                for (int i = 0; i < -dif; i++)
                {
                    int index = System.Array.IndexOf(childPerLayer, Mathf.Min(childPerLayer));
                    new_childPerLayer[index]--;
                    childPerLayer[index]++;
                }
            }
        }

        return new_childPerLayer;
    }

    public Vector3[] getPos(int child, int[] childPerLayer)
    {

        Vector3[] pos = new Vector3[child];

        if (isCenter)
        {
            pos[0] = Vector3.zero;
        }

        int childLayer;
        float ThetaToAdd = 0, Theta;
        float rayonLayer = rayon / (float)Layer;
        float r;
        int index = isCenter ? 1 : 0;
        for (int k = 0; k < childPerLayer.Length; k++)
        {
            childLayer = childPerLayer[k];

            if (childLayer == 1)
            {
                pos[index] = Vector3.zero;
                index++;
                continue;
            }

            ThetaToAdd += (k > 0) ? Mathf.PI / childPerLayer[k - 1] - Mathf.PI / childLayer : 0;
            Theta = 2 * Mathf.PI / childLayer;
            r = rayonLayer * (k + 1);

            for (int i = 0; i < childLayer; i++)
            {
                pos[index] = new Vector3(Mathf.Cos(Theta * i + ThetaToAdd), Mathf.Sin(Theta * i + ThetaToAdd), 0) * r;
                index++;
            }
        }
        return pos;
    }

    public Quaternion[] getRotation(int child, int[] childPerLayer)
    {
        
        Quaternion[] rotation = new Quaternion[child];

        if (isCenter)
        {
            rotation[0] = new Quaternion();
        }

        int childLayer;
        float ThetaToAdd = 0, Theta;
        int index = isCenter ? 1 : 0;

        for (int k = 0; k < childPerLayer.Length; k++)
        {
            childLayer = childPerLayer[k];

            if (childLayer == 1)
            {
                rotation[index] = new Quaternion();
                index++;
                continue;
            }

            ThetaToAdd += (k > 0) ? 180 / childPerLayer[k - 1] - 180 / childLayer : 0;
            Theta = 360 / childLayer;

            for (int i = 0; i < childLayer; i++)
            {
                rotation[index] = Quaternion.Euler(0, 0, 90 - (Theta * i + ThetaToAdd));
                index++;
            }
        }
        return rotation;
    }

    public Vector2 getChildSize(int childLayer, int indexLayer)
    {
        float distance = Mathf.Min(2 * (rayon / Layer) * (indexLayer + 1) * Mathf.Sin(Mathf.PI / childLayer), rayon/Layer) - spacing;
        switch (childFitType)
        {
            case ChildFitType.FixedSize:
                return childSize;
            case ChildFitType.FixedHeight:
                return new Vector2(distance / 1.414f, childSize.y);
            case ChildFitType.FixedWidth:
                return new Vector2(childSize.x, distance / 1.414f);
            case ChildFitType.Square:
                return Vector2.one * distance / 1.414f;
            case ChildFitType.Circle:
                return Vector2.one * distance;
        }
        return Vector2.zero;
    }

    public bool Restriction(int child)
    {
        if (child <= 0) return true;
        switch (layerFitType)
        {
            case LayerFitType.Auto:
                slope = 0;
                intercept = 6;
                firstLayerChildMax = 6;
                break;
            case LayerFitType.Fixed:
                slope = 0;
                intercept = 6;
                firstLayerChildMax = 6;
                if (Layer == 0) return true;
                break;
            case LayerFitType.Custom:
                break;
        }

        if(rayonFitType == RayonFitType.Auto) proportion = 1;

        return false;
    }

    public override void CalculateLayoutInputHorizontal()
    {
        if (refreshEnable)
            Refresh();
    }

    public override void CalculateLayoutInputVertical() { }

    public override void SetLayoutHorizontal() { }

    public override void SetLayoutVertical() { }
}
