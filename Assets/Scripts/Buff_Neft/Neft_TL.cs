using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neft_TL : MonoBehaviour
{
    [Range(0, 1)]
    public float Percent;
    // Start is called before the first frame update
    void Start()
    {
        Buffer.TL_Increase += DecreseTL;
    }

    private void OnDestroy()
    {
        Buffer.TL_Increase -= DecreseTL;
    }

    private void DecreseTL(ref float pc)
    {
        pc -= Percent;
    }
}
