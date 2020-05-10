using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MathUtils
{
    //http://allenchou.net/2015/04/game-math-numeric-springing-examples/
    public static void Spring(ref float x, ref float v, float xt, float zeta, float omega, float deltaTime)
    {
        float f = 1.0f + 2.0f * deltaTime * zeta * omega;
        float oo = omega * omega;
        float hoo = deltaTime * oo;
        float hhoo = deltaTime * hoo;
        float detInv = 1.0f / (f + hhoo);
        float detX = f * x + deltaTime * v + hhoo * xt;
        float detV = v + hoo * (xt - x);
        x = detX * detInv;
        v = detV * detInv;
    }

    public static void SpringVector(ref Vector3 x, ref Vector3 v, Vector3 xt, float zeta, float omega, float deltaTime)
    {
        Spring(ref x.x, ref v.x, xt.x, zeta, omega, deltaTime);
        Spring(ref x.y, ref v.y, xt.y, zeta, omega, deltaTime);
        Spring(ref x.z, ref v.z, xt.z, zeta, omega, deltaTime);
    }

    public static Vector3 GetRandomOffset(Vector3 pos, float radius, float min = 0)
    {
        Vector3 pointOnCircle = Random.insideUnitCircle * radius;
        Vector3 offset = new Vector3(pointOnCircle.x, pos.y, pointOnCircle.y);

        if(min > 0)
        {
            if (offset.sqrMagnitude < min * min)
            {
                offset = offset.normalized * min;
            }
        }

        return pos + offset;
    }

    public static bool IsInRange(Vector3 point, Vector3 target, float range)
    {
        float sqm = (point - target).sqrMagnitude;
        return sqm <= range * range;
    }
}

[System.Serializable]
public struct Delay
{
    public float Min;
    public float Max;

    private float nextDelay;
    private float timestamp;

    public Delay(float min, float max)
    {
        Min = min;
        Max = max;

        timestamp = -max;
        nextDelay = 0.0f;
    }

    public bool IsUp
    {
        get
        {
            return Time.time >= timestamp + nextDelay;
        }
    }

    public float Elapsed
    {
        get
        {
            return Time.time - timestamp;
        }
    }

    public void Next()
    {
        nextDelay = Random.Range(Min, Max);
        timestamp = Time.time;
    }

    public void Next(float min, float max)
    {
        Min = min;
        Max = max; 

        nextDelay = Random.Range(Min, Max);
        timestamp = Time.time;
    }

    public override string ToString()
    {
        return $"RandomDelay: IsUp({IsUp}), Min({Min}), Max({Max}), Next({nextDelay}), Elapsed({Elapsed})";
    }
}
