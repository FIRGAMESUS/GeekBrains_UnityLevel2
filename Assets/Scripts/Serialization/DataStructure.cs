using System;
using UnityEngine;

[Serializable]
public struct SVect3
{
    public float X;
    public float Y;
    public float Z;

    public SVect3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static implicit operator SVect3(Vector3 val)
    {
        return new SVect3(val.x, val.y, val.z);
    }

    public static implicit operator Vector3(SVect3 val)
    {
        return new Vector3(val.X, val.X, val.Z);
    }

}
