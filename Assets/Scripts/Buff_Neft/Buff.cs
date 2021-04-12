using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Buff
{
    public static void BuffToTake(TakeBuff take, int type, float value)
    {
        take.Register(null, type, value);
    }
}
