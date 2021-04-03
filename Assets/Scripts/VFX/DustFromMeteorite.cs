using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustFromMeteorite : DustControl
{
    public Meteorite host;
    public int Mutis = 1;


    protected override void Awake()
    {
        base.Awake();
        host.OnHit += OnHit;
        host.OnDestroy += OnBroken;
    }

    private void OnHit(DamageData damage)
    {
        if (pooling == null)
        {
            Debug.Log("Dust need Prefab");
        }
        Vector2 size = host.size / 2;
        Vector3 DirZ = MathQ.DirectionToRotation(damage.Direction);
        int Amount = damage.Damage / 2 * Mutis;
        float Off = 15;
        for (int i = 0; i < Amount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-size.x, size.x), Random.Range(-size.y, size.y), 0);
            Vector3 dir = MathQ.RotationToDirection(DirZ.z + Random.Range(-Off, Off));
            pos = transform.TransformPoint(pos);
            (pooling.Spawn(id_dust, pos, MathQ.DirectionToQuaternion(new Vector3(0, 0, Random.Range(0, 360)))) as Dust)
                 .SetUp(1, dir, Random.Range(RangeSpeed.x, RangeSpeed.y), Random.Range(RangeSize.x, RangeSize.y), color);
        }
    }

    private void OnBroken()
    {
        if (pooling == null)
        {
            Debug.Log("Dust need Prefab");
            return;
        }
        float DirZ = 0;
        int Amount = (int)(50 * host.size.x * host.size.y);
        float Off = 180;
        for (int i = 0; i < Amount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-RangeSpawn.x, RangeSpawn.x), Random.Range(-RangeSpawn.y, RangeSpawn.y), 0) + Offset;
            Vector3 dir = MathQ.RotationToDirection(DirZ + Random.Range(-Off, Off));
            pos = transform.TransformPoint(pos);
            (pooling.Spawn(id_dust, pos, MathQ.DirectionToQuaternion(new Vector3(0, 0, Random.Range(0, 360)))) as Dust)
                 .SetUp(1, dir, Random.Range(RangeSpeed.x, RangeSpeed.y), Random.Range(RangeSize.x, RangeSize.y), color);
        }
    }
}
