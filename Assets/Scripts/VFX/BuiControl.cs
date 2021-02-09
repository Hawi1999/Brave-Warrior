using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiControl : MonoBehaviour
{
    protected Sprite[] sprites => VFXManager.Instance.SpritesBui;
    public Dust BuiPF => VFXManager.Instance.DustPrefab;
    public Color color;

    protected virtual Vector2 Center
    {
        get
        {
            return transform.position;
        }
    }
    [SerializeField] protected Vector3 Offset;
    [SerializeField] protected Vector2 RangeSpawn;
    [SerializeField] protected Vector2 RangeSpeed;
    [SerializeField] protected Vector2 RangeSize;

    public void SpawnBui(int Amount)
    {
        if (sprites == null || sprites.Length == 0)
        {
            Debug.Log("Không có sprite cho Bụi");
            return;
        }
        for (int i = 0; i < Amount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-RangeSpawn.x, RangeSpawn.x), Random.Range(-RangeSpawn.y, RangeSpawn.y), 0) + (Vector3)Center + Offset;
            Vector3 dir = (pos - (Vector3)Center).normalized;
            Instantiate(BuiPF, pos, MathQ.DirectionToQuaternion(new Vector3(0,0,Random.Range(0,360)))).SetUp(sprites[Random.Range(0, sprites.Length)], 1, dir, Random.Range(RangeSpeed.x, RangeSpeed.y), Random.Range(RangeSize.x, RangeSize.y), color);
        }
    }

    public virtual void SpawnBui(int Amount, int DirZ, int Off)
    {
        if (sprites == null || sprites.Length == 0)
        {
            Debug.Log("Không có sprite cho Bụi");
            return;
        }
        for (int i = 0; i < Amount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-RangeSpawn.x, RangeSpawn.x), Random.Range(-RangeSpawn.y, RangeSpawn.y), 0) + (Vector3)Center + Offset;
            Vector3 dir = MathQ.RotationToDirection(DirZ + Random.Range(-Off, Off));
            Instantiate(BuiPF, pos, MathQ.DirectionToQuaternion(new Vector3(0, 0, Random.Range(0, 360)))).SetUp(sprites[Random.Range(0, sprites.Length)], 1, dir, Random.Range(RangeSpeed.x, RangeSpeed.y), Random.Range(RangeSize.x, RangeSize.y), color);
        }
    }

    public void OnEnemyDamageTook(Enemy e, int dam, DamageElement damdata)
    {
        SpawnBui((dam) / 3 + 1);
    }


}
