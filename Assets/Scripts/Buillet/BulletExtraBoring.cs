using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExtraBoring : BulletBase
{
    [HideInInspector] public Entity skipGameobject;

    protected override void OnHitTarget(ITakeHit take, Vector3 point)
    {
        if (take is TakeDamage && skipGameobject != null)
        {
            if (skipGameobject != null && ((TakeDamage)take).entity != null && ((TakeDamage)take).entity == skipGameobject)
            {
                return;
            }
        }
        base.OnHitTarget(take, point);
    }

    protected override void OnBegin()
    {
        base.OnBegin();
        skipGameobject = null;
    }
}
