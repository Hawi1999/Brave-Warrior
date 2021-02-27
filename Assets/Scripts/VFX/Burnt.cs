using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnt : ElementalBuffBad
{

    private float ThoiGianConLai;
    private float maxTime = 4f;

    private float lastTime;
    private float DelayTime = 0.5f;

    private Entity target;

    private float timedelay = 0.05f;
    private float timedelaycurrent = 0;
    private Sprite[] Sprites => VFXManager.Instance.SpritesDust;
    private Dust bui => VFXManager.Instance.DustPrefab;
    private bool start;

    public static int MaxDamage = 10000000;
    public static int MinDamage = 2;
    public static float Tile = 0.004f;
    private static float CON = 100;
    private void Update()
    {
        if (!start) return;
        if (Time.time - lastTime >= DelayTime)
        {
            SatThuong();
            lastTime = Time.time;
        }
        timedelaycurrent += Time.deltaTime;
        int i = 0;
        while (timedelaycurrent >= timedelay)
        {
            Spawn();
            timedelaycurrent -= timedelay;
            if (i++ == 100)
                break;
        }
        ThoiGianConLai -= Time.deltaTime;
        if (ThoiGianConLai <= 0)
        {
            EndUp();
        }
    }

    public override void StartUp(Entity target, float time)
    {
        start = true;
        this.target = target;
        lastTime = Time.time - DelayTime;
        ThoiGianConLai = time;
        timedelay = 1 / (CON * target.size.x * target.size.y);
        target.OnBuffsChanged?.Invoke(DamageElement.Fire, true);
        target.OnHide += (a) =>
        {
            if (a)
            {
                EndUp();
            }
        };
        }

    public void AddTime(float time)
    {
        ThoiGianConLai = Mathf.Clamp(ThoiGianConLai + time, 0, maxTime);
    }

    private void SatThuong()
    {
        if (target != null)
        {
            DamageData dam = new DamageData();
            dam.Type = DamageElement.Fire;
            dam.FireFrom = true;
            dam.BackForce = 0f;
            target.TakeDamage(dam);
        }
    }
    private void OnDestroy()
    {
        target.OnBuffsChanged?.Invoke(DamageElement.Fire, false);
    }
    public static void Chay(Entity target, float Time)
    {
        Burnt elec;
        if (target.TryGetComponent(out elec))
        {
            elec.AddTime(Time);
        } else
        {
            elec = target.gameObject.AddComponent<Burnt>();
            elec.StartUp(target, Time);
        }
    }
    private void Spawn()
    {
        if (Sprites == null || Sprites.Length == 0)
            return;
        Vector3 Center = target.center;
        Vector3 Size = target.size * 2/3;
        Vector3 position = new Vector3(Random.Range(-Size.x / 2, Size.x / 2), Random.Range(-Size.y / 2, Size.y / 2)) + Center;
        Quaternion rotation = MathQ.DirectionToQuaternion(new Vector3(0, 0, Random.Range(0, 360f)));
        Fire fire = VFXManager.PoolingFire.Spawn(position, rotation);
        fire.SetUp(0.7f, Vector3.up, 1f, 1f, Color.white);
    }
}
