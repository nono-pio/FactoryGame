using UnityEngine;
using UnityEngine.UI;

public class CircleLayout : LayoutGroup
{

    public enum LayerFitType
    {
        Auto,
        Fixed
    }

    public enum RayonFitType
    {
        Auto,
        Proportion,
        Fixed
    }

    public int Layer;
    public LayerFitType layerFitType;

    public bool useCenter;
    public bool isCenter = false;

    public float rayon;
    public RayonFitType rayonFitType;
    [Range(0, 1)] public float proportion; 

    public Vector2 childSize;

    public bool setRotation;

    public bool refreshEnable = true;

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

        if(childPerLayer == null) return;

        Vector3[] position = getPos(child, childPerLayer);
        Quaternion[] rotation = null;
        if (setRotation) rotation = getRotation(child, childPerLayer);

        Vector2 restSize = getRest(realSizeParent);

        for (int i = 0; i < child; i++)
        {
            var item = rectChildren[i];
            if (setRotation) item.rotation = rotation[i];
            SetChildAlongAxis(item, 0, position[i].x + rayon + restSize.x, childSize.x);
            SetChildAlongAxis(item, 1, position[i].y + rayon + restSize.y, childSize.y);
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
                Layer = Mathf.CeilToInt((Mathf.Sqrt(1 + 4 / 3 * child) - 1) / 2);
                break;
            case LayerFitType.Fixed:
                break;
        }
    }

    public int[] getChildPerLayer(int child)
    {

        if(useCenter)
        {
            child--;
            if (Mathf.RoundToInt(2 * (child / (float)(Layer * (Layer + 1)))) >= 6)
            {
                isCenter = true;
            }
            else
            {
                isCenter = false;
                child++;
            }
        } else
            isCenter = false;

        float[] childPerLayer = new float[Layer];
        childPerLayer[0] = 2 * (child / (float)(Layer * (Layer + 1)));

        for (int k = 1; k < Layer; k++)
        {
            childPerLayer[k] = (k + 1) * childPerLayer[0];
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

    public bool Restriction(int child)
    {
        if (child <= 0) return true;
        if (layerFitType == LayerFitType.Fixed && Layer <= 0) return true;

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
