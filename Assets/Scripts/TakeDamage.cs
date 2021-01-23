using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : TakeHit
{
    public Entity entity;
    public override void TakeDamaged(DamageData dama)
    {
        entity.TakeDamage(dama);
    }
}
