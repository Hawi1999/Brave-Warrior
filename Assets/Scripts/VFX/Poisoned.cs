using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poisoned : ElementalBuffBad
{
    private float ThoiGianConLai;
    private float maxTime = 6f;

    private float lastTime;
    private float DelayTime = 2f;

    private Entity target;

    private float timedelaySpawn = 0.05f;
    private float timedelaycurrent = 0;
    private bool start;

    public static int MinDamage = 4;
    public static int MaxDamage = 1000000;
    public static float Tile = 0.01f;
    private static int CON = 60;
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
        while (timedelaycurrent >= timedelaySpawn)
        {
            timedelaycurrent -= timedelaySpawn;
            if (i++ == 1000)
            {
                break;
            }
        }
        Debug.Log(ThoiGianConLai);
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
        timedelaySpawn = 1 / (CON * target.size.x * target.size.y);
        target.OnBuffsChanged?.Invoke(DamageElement.Poison, true);
    }

    private void OnDestroy()
    {
        target.OnBuffsChanged?.Invoke(DamageElement.Poison, false);
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
            dam.Type = DamageElement.Poison;
            dam.PoisonFrom = true;
            target.TakeDamage(dam);
        }
    }

    public static void NhiemDoc(Entity target, float Time)
    {
        Poisoned poison;
        if (target.TryGetComponent(out poison))
        {
            poison.AddTime(Time);
        } else
        {
            poison = target.gameObject.AddComponent<Poisoned>();
            poison.StartUp(target, Time);
        }
    }
}
