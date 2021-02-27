using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustControl : MonoBehaviour
{
    protected Sprite[] sprites => VFXManager.Instance.SpritesDust;
    public Dust BuiPF => VFXManager.Instance.DustPrefab;
    public Color color;


    public bool ShowGizmos = true;

    protected virtual void Awake()
    {
        FixToSwap(ref RangeSpeed);
        FixToSwap(ref RangeSize);
    }

    protected void FixToSwap(ref Vector2 vector2)
    {
        if (vector2.x > vector2.y)
        {
            vector2 = new Vector2(vector2.y, vector2.x);
        }
    }

    protected virtual Vector2 Center
    {
        get
        {
            return transform.position;
        }
    }
    [SerializeField] protected Vector3 Offset;
    [SerializeField] protected Vector2 RangeSpawn = new Vector2(1, 1);
    [SerializeField] protected Vector2 RangeSpeed = new Vector2(1, 1.5f);
    [SerializeField] protected Vector2 RangeSize = new Vector2(0.5f, 1);

    public void SpawnBui(int Amount)
    {
        if (VFXManager.PoolingDust == null)
        {
            Debug.Log("VFXManagerPoolingDust does not found");
            return;
        }
        for (int i = 0; i < Amount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-RangeSpawn.x, RangeSpawn.x), Random.Range(-RangeSpawn.y, RangeSpawn.y), 0) + (Vector3)Center + Offset;
            Vector3 dir = (pos - (Vector3)Center).normalized;
            VFXManager.PoolingDust.Spawn(pos, MathQ.DirectionToQuaternion(new Vector3(0,0,Random.Range(0,360))))
                .SetUp(1, dir, Random.Range(RangeSpeed.x, RangeSpeed.y), Random.Range(RangeSize.x, RangeSize.y), color);
        }
    }

    public virtual void SpawnBui(int Amount, int DirZ, int Off)
    {
        for (int i = 0; i < Amount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-RangeSpawn.x, RangeSpawn.x), Random.Range(-RangeSpawn.y, RangeSpawn.y), 0) + (Vector3)Center + Offset;
            Vector3 dir = MathQ.RotationToDirection(DirZ + Random.Range(-Off, Off));
            VFXManager.PoolingDust.Spawn(pos, MathQ.DirectionToQuaternion(new Vector3(0, 0, Random.Range(0, 360))))
                .SetUp(1, dir, Random.Range(RangeSpeed.x, RangeSpeed.y), Random.Range(RangeSize.x, RangeSize.y), color);
        }
    }

    protected virtual void OnDrawGizmos()
    {
        if (!ShowGizmos)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector3)Center + Offset, RangeSpawn * 2);
        Gizmos.color = Color.white;
        Vector3 tam1 = transform.position + new Vector3(-1.5f, 1.5f, 0);
        Vector3 tam2 = transform.position + new Vector3(1.5f, 1.5f, 0);
        Gizmos.DrawWireSphere(tam1, 0.25f * RangeSize.x);
        Gizmos.DrawLine(tam1 + new Vector3(RangeSize.x * 0.25f, 0, 0), tam2 + new Vector3(RangeSize.y * 0.25f, 0, 0));
        Gizmos.DrawWireSphere(tam2, 0.25f * RangeSize.y);
        Gizmos.color = color;
        Gizmos.DrawWireSphere((Vector3)Center + Offset, RangeSpeed.x); 
        Gizmos.DrawWireSphere((Vector3)Center + Offset, RangeSpeed.y);
    }

    protected virtual void OnValidate()
    {

    }
}
