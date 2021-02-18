using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletEnemyTakeHit : BulletTakeHit
{
    BulletEnemy bulletEnemy => GetComponent<BulletEnemy>();

    public override void TakeDamaged(DamageData data)
    {
        bulletEnemy?.TakeDamage(data);
    }
}
