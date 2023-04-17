using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RangeEnum { Zero = 0, Close = 100, Mid = 200, Far = 300 };

public static class RangeColors
{
    public static Color NeonGreen() => new Color(0.25f, 0.89f, 0f); // green
    public static Color NeonYellow() => new Color(1f, 1f, 0f); // yelow
    public static Color NeonRed() => new Color(1f, 0.192f, 0.192f); // red

    public static Color GetColorForDistance(float distance)
    {
        if (distance <= (int)RangeEnum.Close)
        { return NeonGreen(); }
        else if (distance <= (int)RangeEnum.Mid)
        { return NeonYellow(); }
        else if (distance <= (int)RangeEnum.Far)
        { return NeonRed(); }
        else
        { return Color.white; }
    }

    
};

