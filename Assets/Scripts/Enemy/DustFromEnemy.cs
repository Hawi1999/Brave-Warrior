using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustFromEnemy : DustControl
{
    public Enemy enemy;


    [SerializeField] private int MutiDus = 2;

    protected override void Awake()
    {
        base.Awake();
        if (enemy == null)
        {
            enemy = GetComponentInParent<Enemy>();
        }
        if (enemy != null)
        {
            enemy.OnTookDamage += SpawnBui;
            enemy.OnDeath += (Entity) => Dead();
        }

    }
    public virtual void SpawnBui(DamageData damage)
    {
        Vector3 DirZ = MathQ.DirectionToRotation(damage.Direction);
        int Amount = damage.Damage/2 * MutiDus;
        float Off = 15;
        for (int i = 0; i < Amount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-RangeSpawn.x, RangeSpawn.x), Random.Range(-RangeSpawn.y, RangeSpawn.y), 0) + Offset;
            Vector3 dir = MathQ.RotationToDirection(DirZ.z + Random.Range(-Off, Off));
            pos = transform.TransformPoint(pos);
            VFXManager.PoolingDust.Spawn(pos, MathQ.DirectionToQuaternion(new Vector3(0, 0, Random.Range(0, 360))))
                 .SetUp(1, dir, Random.Range(RangeSpeed.x, RangeSpeed.y), Random.Range(RangeSize.x, RangeSize.y), color);
        }
    }

    public virtual void Dead()
    {
        float DirZ = 0;
        int Amount = 60;
        float Off = 180;
        for (int i = 0; i < Amount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-RangeSpawn.x, RangeSpawn.x), Random.Range(-RangeSpawn.y, RangeSpawn.y), 0) + Offset;
            Vector3 dir = MathQ.RotationToDirection(DirZ + Random.Range(-Off, Off));
            pos = transform.TransformPoint(pos);
            VFXManager.PoolingDust.Spawn(pos, MathQ.DirectionToQuaternion(new Vector3(0, 0, Random.Range(0, 360))))
                 .SetUp(1, dir, Random.Range(RangeSpeed.x, RangeSpeed.y), Random.Range(RangeSize.x, RangeSize.y), color);
        }
    }

}
