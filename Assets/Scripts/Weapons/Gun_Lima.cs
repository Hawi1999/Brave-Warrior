using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Lima : GunBase
{
    [SerializeField] Transform TFRadanPhai;
    [SerializeField] float timeAdd;

    public override Vector3 PositionStartAttack
    {
        get
        {
            return TFRadanPhai.position;
        }
    }

    protected override void SetUpDamageData(DamageData dam, Vector3 Direction)
    {
        base.SetUpDamageData(dam, Direction);
        dam.Type = DamageElement.Electric;
        dam.timeGiatDien = timeAdd;
    }
}
