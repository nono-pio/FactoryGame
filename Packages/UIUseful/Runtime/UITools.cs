using UnityEngine;

public static class UITools
{
    public static Vector2 TextAnchorPosition(TextAnchor textAnchor)
    {
        return new Vector2((float) ((int) textAnchor % 3 ) / 2, (float) ((int) textAnchor / 3) / 2);
    }

    public static float firstDegreeRoot(float a, float b)
    {
        return - b/a;
    }

    public static Vector2[] secondDegreeRoot(float a, float b, float c)
    {
        if (a == 0) return new Vector2[]{ new Vector2(firstDegreeRoot(b,c), 0) };

        float delta = b*b - 4*a*c;

        Vector2[] roots = new Vector2[2];
        roots[0].x = -b;
        roots[1].x = -b;

        if (delta < 0)
        {
            float sqrt = Mathf.Sqrt(-delta);
            roots[0].y = sqrt;
            roots[1].y = -sqrt;
        }
        else
        {
            float sqrt = Mathf.Sqrt(delta);
            roots[0].x += sqrt;
            roots[1].x += -sqrt;
        }
        
        roots[0] /= (2*a);
        roots[1] /= (2*a);

        return roots;
    }

    public static Vector2[] thirdDegreeRoot(float a, float b, float c, float d)
    {
        if (a == 0) return secondDegreeRoot(b, c, d);

        float newC = c - b*b/(3*a);
        float newD = 2/27 * b*b*b/(a*a) - c*b/(3*a) + d ;

        Vector2[] rootsY = thirdDegreeRootReduct(newC / a, newD / a);
        Vector2[] roots = new Vector2[rootsY.Length];

        Vector2 yToX = new Vector2(- b/(3*a), 0);
        for (int i = 0; i < rootsY.Length; i++)
        {
            roots[i] = rootsY[i] + yToX;
        }

        return round(roots, 5);
    }

    private static Vector2[] thirdDegreeRootReduct(float c, float d)
    {
        Vector2[] z = secondDegreeRoot(1, d, - c*c*c / 27);
        int indexZ = z[0] == Vector2.zero ? 1 : 0;

        float theta = Vector2.SignedAngle(z[indexZ], Vector2.right) * 0.01745329f;
        float r = z[indexZ].magnitude;

        Vector2[] roots = new Vector2[3];

        float R = System.MathF.Cbrt(r);
        float R2 = - c / (3 * R);

        float deltaTheta = Mathf.PI * 2/3;
        float thetaN = theta * 1/3 - deltaTheta;
        for (int n = -1; n < 2; n++)
        {
            roots[n + 1] = new Vector2((R + R2) * Mathf.Cos(thetaN), (R - R2) * Mathf.Sin(thetaN));

            if (n != 1) thetaN += deltaTheta;
        }

        return roots;
    }

    public static Vector2[] round(Vector2[] data, int _decimal)
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i].x = System.MathF.Round(data[i].x, _decimal);
            data[i].y = System.MathF.Round(data[i].y, _decimal);
        }
        return data;
    }
}

public enum ChildUse
{
    ActiveChild,
    AllChildren,
    FixedChild
}
