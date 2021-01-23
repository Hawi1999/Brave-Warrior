using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_TL : MonoBehaviour
{
    [Range(0, 1)]
    public float Percent;
    // Start is called before the first frame update
    void Start()
    {
        Buffer.TL_Increase += IncreseTL;
    }

    private void OnDestroy()
    {
        Buffer.TL_Increase -= IncreseTL;
    }

    private void IncreseTL(ref float pc)
    {
        pc += Percent;
    }
}
