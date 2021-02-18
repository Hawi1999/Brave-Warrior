using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_K1 : GunBase
{
    [SerializeField] Transform TFRadanPhai;

    public override Vector3 PositionStartAttack
    {
        get
        {
            return TFRadanPhai.position;
        }
    }
}
