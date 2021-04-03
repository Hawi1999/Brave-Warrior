using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustFromPlayer : DustControl
{
    public PlayerController player;

    protected override void Awake()
    {
        base.Awake();
        player.OnDeath += (a) => WhenPlayerDie();
    }

    private void WhenPlayerDie()
    {

        if (pooling == null)
        {
            Debug.Log("Dust need Prefab");
            return;
        }
        float DirZ = 0;
        int Amount = 60;
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
