using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neft_Dia : MonoBehaviour
{
    [Range(0, 1)]
    public float Percent;
    // Start is called before the first frame update
    void Start()
    {
        Buffer.Dia_Increase += DecreseDia;
    }

    private void OnDestroy()
    {
        Buffer.Dia_Increase -= DecreseDia;
    }

    private void DecreseDia(ref float pc)
    {
        pc -= Percent;
    }
}
