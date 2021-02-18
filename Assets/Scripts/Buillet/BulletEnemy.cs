using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BulletEnemyTakeHit))]
[RequireComponent(typeof(BoxCollider2D))]
public class BulletEnemy : BulletBase
{

    public void TakeDamage(DamageData damage)
    {
        if (damage.FromMeleeWeapon)
        {
            Destroyed();
        }
    }
}
