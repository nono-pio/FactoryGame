using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UITools
{
    public static Vector2 TextAnchorPosition(TextAnchor textAnchor)
    {
        return new Vector2((float) ((int) textAnchor % 3 ) / 2, (float) ((int) textAnchor / 3) / 2);
    }
}

public enum ChildUse
{
    ActiveChild,
    AllChildren,
    FixedChild
}
