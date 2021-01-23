using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_K1 : GunBase
{
    [SerializeField] Transform TFRadanPhai;

    // false cho dưới và 1 cho trên
    public override Vector3 viTriRaDan
    {
        get
        {
            return TFRadanPhai.position;
        }
    }
}
