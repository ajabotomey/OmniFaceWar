// Download: https://www.patreon.com/posts/3301589
// Obtained 1/08/2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SafeFloat
{
    private float offset;
    private float value;

    public SafeFloat(float value = 0)
    {
        offset = Random.Range(-1000, +1000);
        this.value = value + offset;
    }

    public float GetValue()
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

    public static SafeFloat operator +(SafeFloat f1, SafeFloat f2) => new SafeFloat(f1.GetValue() + f2.GetValue());

    public static SafeFloat operator -(SafeFloat f1, SafeFloat f2) => new SafeFloat(f1.GetValue() - f2.GetValue());

    public static SafeFloat operator *(SafeFloat f1, SafeFloat f2) => new SafeFloat(f1.GetValue() * f2.GetValue());

    public static SafeFloat operator /(SafeFloat f1, SafeFloat f2) => new SafeFloat(f1.GetValue() / f2.GetValue());

    public static bool operator ==(SafeFloat f1, SafeFloat f2) => f1.GetValue() == f2.GetValue();

    public static bool operator !=(SafeFloat f1, SafeFloat f2) => f1.GetValue() != f2.GetValue();

    public static bool operator <=(SafeFloat f1, SafeFloat f2) => f1.GetValue() <= f2.GetValue();

    public static bool operator >=(SafeFloat f1, SafeFloat f2) => f1.GetValue() >= f2.GetValue();

    public static implicit operator SafeFloat(int ii) => new SafeFloat(ii);

    public static implicit operator SafeFloat(float fi) => new SafeFloat(fi);

    public static implicit operator float(SafeFloat si) => si.GetValue();
}
