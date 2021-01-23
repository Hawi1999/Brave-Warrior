using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neft_DOLA : MonoBehaviour
{
    [Range(0, 1)]
    public float Percent;
    // Start is called before the first frame update
    void Start()
    {
        Buffer.DOLA_Increase += DecreseDOLA;
    }

    private void OnDestroy()
    {
        Buffer.DOLA_Increase -= DecreseDOLA;
    }

    private void DecreseDOLA(ref float pc)
    {
        pc -= Percent;
    }
}
