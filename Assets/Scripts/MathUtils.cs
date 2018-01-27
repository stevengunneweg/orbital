using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class MathUtils
{
    public static readonly float RADIUS2DEGREE = 180 / Mathf.PI;
    public static float AngleDifference(float angleA, float angleB)
    {
        float difference = (angleA - angleB + 360) % 360;
        if (difference > 180)
            difference = 360 - difference;
        return difference;
    }
}
