using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Lima : GunBase
{
    [SerializeField] Transform TFRadanPhai;
    [SerializeField] float timeAdd;

    public override Vector3 viTriRaDan
    {
        get
        {
            return TFRadanPhai.position;
        }
    }

    protected override DamageData setUpDamageData()
    {
        DamageData dam = base.setUpDamageData();
        dam.Type = DamageElement.Electric;
        dam.timeGiatDien = timeAdd;
        return dam;
    }
}
