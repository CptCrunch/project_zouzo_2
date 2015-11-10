using UnityEngine;
using System.Collections;

public static class Util{

    public static float Ease(float x, float easeAmount)
    {
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }
}
