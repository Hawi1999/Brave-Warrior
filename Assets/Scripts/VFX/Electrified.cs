using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electrified : ElementalBuffBad
{
    bool isInElectricShock = false;
    private float ThoiGianConLai;
    private float maxTime = 6f;

    private float lastTime;
    private float DelayTime = 1.5f;

    private float timeGiat = 0.5f;
    private float lastTimeGiat;

    private bool start;
    private Entity target;

    bool avalableRender = true;
    private void Start()
    {
        lastTime = -DelayTime;
    }

    private void Update()
    {
        if (!start) return;

        if (!isInElectricShock)
        {
            if (Time.time - lastTime > DelayTime && target != null)
            {
                isInElectricShock = true;
                target.LockAttack.Register("Electrical");
                target.LockMove.Register("Electrical");
                if (avalableRender)
                {
                    ShockWave();
                }
                lastTimeGiat = Time.time;
            }
        } else
        {
            if (Time.time - lastTimeGiat > timeGiat && target != null)
            {
                isInElectricShock = false;
                target.LockAttack.CancelRegistration("Electrical");
                target.LockMove.CancelRegistration("Electrical");
                lastTime = Time.time;
            }
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
        ThoiGianConLai = time;
        isInElectricShock = false;
        lastTimeGiat = Time.time - timeGiat;
        if (target != null)
        {
            target.Harmful_Electric = true;
            target.OnValueChanged?.Invoke(Entity.HARMFUL_ELECTIC);
            target.OnHide += () => Setavalible(true);
            target.OnAppear += () => Setavalible(false);
            target.OnIntoTheGound += () => Setavalible(true);
            target.OnOuttoTheGound += () => Setavalible(false);
            target.OnDeath += (a) => WhenTargetDied();
        }
    }

    public void AddTime(float time)
    {
        ThoiGianConLai = Mathf.Clamp(ThoiGianConLai + time, 0, maxTime);
    }

    private void ShockWave()
    {
        VFXManager.GiatDien(target.transform,target.GetPosition(), timeGiat);
    }

    public static void Shockwave(Entity target, float Time)
    {
        Electrified elec = target.gameObject.GetComponent<Electrified>();
        if (elec == null)
        {
            elec = target.gameObject.AddComponent<Electrified>();
            elec.StartUp(target, Time);
        }
        else
        {
            elec.AddTime(Time);
        }
    }

    private void Setavalible(bool a)
    {
        avalableRender = !a;
    }

    private void WhenTargetDied()
    {
        EndUp();
    }

    public override void EndUp()
    {
        base.EndUp();
        if (target != null)
        {
            target.Harmful_Electric = false;
            target.OnValueChanged?.Invoke(Entity.HARMFUL_ELECTIC);
            target.OnHide -= () => Setavalible(true);
            target.OnAppear -= () => Setavalible(false);
            target.OnIntoTheGound -= () => Setavalible(true);
            target.OnOuttoTheGound -= () => Setavalible(false);
            target.LockAttack.CancelRegistration("Electrical");
            target.LockMove.CancelRegistration("Electrical");
            target.OnDeath -= (a) => WhenTargetDied();
        }
    }
}
