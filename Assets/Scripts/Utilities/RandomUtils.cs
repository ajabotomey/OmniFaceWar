using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomUtils
{
    private static ulong seed = (ulong)Random.Range(10000000, 99999999);

    public static float GaussianRandomRange(int min, int max)
    {
        double sum = 0;

        for (int i = min; i < max; i++) {
            ulong holdseed = seed;
            seed ^= seed << 13;
            seed ^= seed >> 17;
            seed ^= seed << 5;
            long r = (long)(holdseed * seed);
            sum += r * (1.0 / 0x7FFFFFFFFFFFFFFF);
        }

        return (float)sum;
    }
}
