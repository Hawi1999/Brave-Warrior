using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Shockick : GunBase
{
    [SerializeField] Transform TFRadanPhai;
    [SerializeField] float TimeAddFire = 2f;

    public override Vector3 PositionStartAttack
    {
        get
        {
            if (TFRadanPhai == null)
            {
                return base.PositionStartAttack;
            }
            else
            {
                return TFRadanPhai.position;
            }
        }
    }

    protected override void SetUpDamageData(DamageData damageData)
    {
        base.SetUpDamageData(damageData);
        damageData.Type = DamageElement.Fire;
        damageData.FireTime = TimeAddFire;
        damageData.FireRatio = 1;
    }
}
