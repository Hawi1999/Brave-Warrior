using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBulletCircle : SpawnBullet
{
    public BulletBase bulletPrefab;
    public int Amount = 4;
    public Transform SpawnBullet;
    public float timeDelaySpawn = 1f;
    [Header("VFX Contruct")]
    public bool AutoStart = false;
    public float timeStart = 1;

    [Header("VFX Detruct")]
    public bool AutoDetruct = false;
    public float timeDetruct = 5;

    [SerializeField] Transform vfxs;


    float lastTimeSpawn = 0;


    private float timeAlive = 0;
    private float DistanceSpawn
    {
        get
        {
            if (SpawnBullet == null)
            {
                return 0;
            }
            else
            {
                return Vector2.Distance(SpawnBullet.position, transform.position);
            }
        }
    }

    private Vector3 GetPositionSpawn(Vector3 dir)
    {
        return transform.position + dir * DistanceSpawn;
    }

    PoolingGameObject pool => PoolingGameObject.PoolingMain;
    private int id_bul;
    protected override void Awake()
    {
        base.Awake();
        if (bulletPrefab != null)
        {
            id_bul = pool.AddPrefab(bulletPrefab);
        }
        spawning = StartWhenAwake;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        pool.RemovePrefab(id_bul);
    }

    private void Start()
    {
        if (spawning)
        {
            lastTimeSpawn = Time.time - timeDelaySpawn;
        }
        if (vfxs != null)
        {
            iTween.ValueTo(gameObject, iTween.Hash(
                    "from", 0f,
                    "to", 1f,
                    "onupdate", "UD"));
        }
        if (AutoStart)
        {
            Invoke("StartSpawn", timeStart);
        }




    }
    private void Update()
    {
        if (!spawning)
            return;
        if (Time.time - lastTimeSpawn > timeDelaySpawn)
        {
            Spawn();
            lastTimeSpawn = Time.time;
        }
        if (AutoDetruct)
        {
            timeAlive += Time.deltaTime;
            if (timeAlive > timeDetruct)
            {
                End();
            }
        }
    }

    private void End()
    {
        spawning = false;
        if (vfxs != null)
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", 1f,
                "to", 0,
                "onupdate", "UD",
                "oncomplete", "CPEnd"));
        else
        {
            CPEnd();
        }

    }

    private void UD(float a)
    {
        for (int i = 0; i < vfxs.childCount; i++)
        {
            Transform tf = vfxs.GetChild(i);
            tf.localScale = Vector3.one * a;
        }
    }

    private void CPEnd()
    {
        Destroy(this.gameObject);
    }

    void Spawn()
    {
        if (bulletPrefab == null || !spawning)
            return;
        float z = Random.Range(0, 360 / Amount);
        for (int i = 0; i < Amount; i++)
        {
            float newZ = z + 360 * i / Amount;
            if (newZ > 180)
            {
                newZ -= 360;
            }
            Vector3 dir = MathQ.RotationToDirection(newZ);
            BulletBase bul = pool.Spawn(id_bul, GetPositionSpawn(dir), Quaternion.Euler(MathQ.DirectionToRotation(dir)), null) as BulletBase;
            DamageData damage = SetUpDamageData();
            damage.Direction = dir;
            bul.StartUp(damage);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < Amount; i++)
        {
            float z = 360 * i / Amount;
            Gizmos.DrawLine(GetPositionSpawn(MathQ.RotationToDirection(z)), MathQ.RotationToDirection(z) * 10 + transform.position);
        }
    }
    public override void StartSpawn()
    {
        base.StartSpawn();
        lastTimeSpawn = Time.time - timeDelaySpawn;
    }
}
