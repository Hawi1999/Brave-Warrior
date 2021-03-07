using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoring : BulletBase
{
    [SerializeField] private BulletBase bulletExtra;
    [SerializeField] private int AmountBulletExtra = 5;
    protected PoolingGameObject<BulletBase> pooling;
    protected override void Awake()
    {
        base.Awake();
        if (bulletExtra != null)
        {
            pooling = new PoolingGameObject<BulletBase>(bulletExtra);
        }
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
            BulletBase bull = pooling.Spawn(transform.position, Quaternion.identity);
            bull.StartUp(damage);
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
        pooling?.DestroyAll();
    }
}
