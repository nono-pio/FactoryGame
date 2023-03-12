using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLayout : MonoBehaviour
{
    public int fixedChild;
    public int Layer;

    public int rayon;

    public GameObject prefabTospawn;
    public Transform parent;

    [ContextMenu("Instantiate")]
    public void test()
    {
        Vector3[] position = getPos(fixedChild);

        foreach (var pos in position)
        {
            Instantiate(prefabTospawn, pos + transform.position, new Quaternion(), parent);
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(parent.GetChild(i).gameObject);
        }
    }

    public void getLayer(int child)
    {
        Layer = Mathf.CeilToInt((Mathf.Sqrt(1 + 4/3 * child) - 1) / 2);
    }

    public int[] getChildPerLayer(int child)
    {
        float[] childPerLayer = new float[Layer];
        childPerLayer[0] = 2 * (child / (float) (Layer * (Layer + 1)));

        for (int k = 1; k < Layer; k++)
        {
            childPerLayer[k] = (k + 1) * childPerLayer[0];
        }

        int[] new_childPerLayer = new int[Layer];
        for (int i = 0; i < Layer; i++)
        {
            new_childPerLayer[i] = Mathf.RoundToInt(childPerLayer[i]);
        }
        return new_childPerLayer;
    }

    public Vector3[] getPos(int child)
    {
        getLayer(child);
        int[] childPerLayer = getChildPerLayer(child);

        Vector3[] pos = new Vector3[child];
        int childLayer;
        float ThetaToAdd = 0, Theta;
        float rayonLayer = rayon / (float) Layer;
        float r;
        int index = 0;
        for (int k = 0; k < childPerLayer.Length; k++)
        {
            childLayer = childPerLayer[k];

            if (childLayer == 1)
            {
                pos[index] = Vector2.zero;
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

}
