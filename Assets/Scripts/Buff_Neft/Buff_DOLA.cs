using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Buff_DOLA : MonoBehaviour
{
    [Range(0, 1)]
    public float Percent;
    // Start is called before the first frame update
    void Start()
    {
        Buffer.DOLA_Increase += IncreseDOLA;
    }

    private void OnDestroy()
    {
        Buffer.DOLA_Increase -= IncreseDOLA;
    }

    private void IncreseDOLA(ref float pc) {
        pc += Percent;
    }
}
