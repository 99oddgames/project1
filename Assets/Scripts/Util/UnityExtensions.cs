using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtensions
{
    public static bool IsInRange(this Transform transform, Vector3 point, float range)
    {
        return MathUtils.IsInRange(transform.position, point, range);
    }
}
