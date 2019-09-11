using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extensions
{
    public static Vector3Int toVector3Int(this Vector2Int v)
    {
        return new Vector3Int(v.x, v.y, 0);
    }

    public static Vector2Int toVector2Int(this Vector3Int v)
    {
        return new Vector2Int(v.x, v.y);
    }

    public static Vector2 tileCenter(this Vector2Int v)
    {
        return new Vector2(v.x + 0.5f, v.y + 0.5f);
    }

    public static Vector2 tileCenter(this Vector3Int v)
    {
        return new Vector2(v.x + 0.5f, v.y + 0.5f);
    }

    public static Vector2Int toVector2IntRound(this Vector3 v)
    {
        return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    public static Vector3Int toVector3IntRound(this Vector3 v)
    {
        return new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
    }

    public static Vector3Int toVector3IntFloor(this Vector3 v)
    {

        return new Vector3Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y), Mathf.FloorToInt(v.z));
    }

    public static Vector2 toVector2(this Vector2Int v)
    {
        return v;
    }

    public static Vector2 toVector2(this Vector3 v)
    {
        return v;
    }

    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }

    public static Vector3 toVector3(this Vector2Int v)
    {
        return new Vector3(v.x, v.y);
    }

    public static Vector2Int toVector2IntFloor(this Vector2 v)
    {
        return new Vector2Int(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y));
    }
}
