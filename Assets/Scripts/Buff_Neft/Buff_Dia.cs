using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Dia : MonoBehaviour
{
    [Range(0, 1)]
    public float Percent;
    // Start is called before the first frame update
    void Start()
    {
        Buffer.Dia_Increase += IncreseDia;
    }

    private void OnDestroy()
    {
        Buffer.Dia_Increase -= IncreseDia;
    }

    private void IncreseDia(ref float pc)
    {
        pc += Percent;
    }
}
