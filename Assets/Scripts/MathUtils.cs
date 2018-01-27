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
        float difference = (angleA - angleB + 360) % 180;
        if (difference > 180)
            difference = (difference + 180)%360;
        return difference;
    }
}
