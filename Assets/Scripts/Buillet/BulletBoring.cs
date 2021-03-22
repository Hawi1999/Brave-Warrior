using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoring : BulletBase
{
    [SerializeField] private BulletExtraBoring bulletExtra;
    [SerializeField] private int AmountBulletExtra = 5;
    protected PoolingGameObject pooling => PoolingGameObject.PoolingMain;
    protected int id_pooling;

    private Entity targetHit;
    protected override void Awake()
    {
        base.Awake();
        if (bulletExtra != null)
        {
            id_pooling = pooling.AddPrefab(bulletExtra);
        }
    }

    protected override void OnHitTarget(ITakeHit take, Vector3 point)
    {
        if (take is TakeDamage)
        {
            targetHit = ((TakeDamage)take).entity;
        }
        base.OnHitTarget(take, point);
    }

    protected override void OnBegin()
    {
        targetHit = null;
        base.OnBegin();
    }

    protected override void OnAfterDestroyed()
    {
        Spawn();
        base.OnAfterDestroyed();
    }

    private void Spawn()
    {
        if (pooling == null)
            return;
        float startAngle = MathQ.DirectionToRotation(damage.Direction).z + Random.Range(0, 360/AmountBulletExtra);
        for (int i = 0; i < AmountBulletExtra; i++)
        {
            DamageData damage = this.damage.Clone;
            float angle = startAngle + 360 / AmountBulletExtra * i;
            if (angle > 180)
            {
                angle -= 360;
            }
            Vector3 dir = MathQ.RotationToDirection(angle);
            damage.Direction = dir;
            BulletExtraBoring bull =  pooling.Spawn(id_pooling, transform.position, Quaternion.identity) as BulletExtraBoring;
            bull.StartUp(damage);
            bull.skipGameobject = targetHit;
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        float startAngle = 0;
        for (int i = 0; i < AmountBulletExtra; i++)
        {
            float angle = startAngle + 360 / AmountBulletExtra * i;
            if (angle > 180)
            {
                angle -= 360;
            }
            Vector3 dir = MathQ.RotationToDirection(angle);
            Gizmos.DrawLine(transform.position, transform.position + dir * 3f);
        }
    }

    protected override void OnDestroy()
    {
        pooling.RemovePrefab(id_pooling);
    }
}
