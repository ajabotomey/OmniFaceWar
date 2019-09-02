using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SafeInt
{
    private int offset;
    private int value;

    public SafeInt(int value = 0)
    {
        offset = Random.Range(-1000, +1000);
        this.value = value + offset;
    }

    public int GetValue()
    {
        return value - offset;
    }

    public void Dispose()
    {
        offset = 0;
        value = 0;
    }

    public override string ToString()
    {
        return GetValue().ToString();
    }

    public static SafeInt[] CopyArray(int[] array)
    {
        if (array == null)
            return null;

        SafeInt[] returnArray = new SafeInt[array.Length];

        for (int i = 0; i < array.Length; i++) {
            returnArray[i] = new SafeInt(array[i]);
        }

        return returnArray;
    }

    public static SafeInt operator +(SafeInt f1, SafeInt f2) => new SafeInt(f1.GetValue() + f2.GetValue());

    public static SafeInt operator -(SafeInt f1, SafeInt f2) => new SafeInt(f1.GetValue() - f2.GetValue());

    public static SafeInt operator *(SafeInt f1, SafeInt f2) => new SafeInt(f1.GetValue() * f2.GetValue());

    public static SafeInt operator /(SafeInt f1, SafeInt f2) => new SafeInt(f1.GetValue() / f2.GetValue());

    public static bool operator ==(SafeInt f1, SafeInt f2) => f1.GetValue() == f2.GetValue();

    public static bool operator !=(SafeInt f1, SafeInt f2) => f1.GetValue() != f2.GetValue();

    public static bool operator <=(SafeInt f1, SafeInt f2) => f1.GetValue() <= f2.GetValue();

    public static bool operator >=(SafeInt f1, SafeInt f2) => f1.GetValue() >= f2.GetValue();

    public static implicit operator SafeInt(int ii) => new SafeInt(ii);

    public static implicit operator SafeInt(float ii) => new SafeInt(UnityEngine.Mathf.RoundToInt(ii));

    public static implicit operator int(SafeInt si) => si.GetValue();
}
