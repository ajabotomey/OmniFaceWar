using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static T First<T>(this List<T> list)
    {
        return list[0];
    }

    public static T Last<T>(this List<T> list)
    {
        return list[list.Count - 1];
    }

    public static void Pop<T>(this List<T> list)
    {
        list.RemoveAt(0);
    }
}
