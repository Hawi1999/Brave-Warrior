﻿using System.Collections;
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
            if (Time.time - lastTime > DelayTime)
            {
                isInElectricShock = true;
                target.OnCheckForAttack += LockAttack;
                target.OnCheckForMove += LockMove;
                if (avalableRender)
                {
                    ShockWave();
                }
                lastTimeGiat = Time.time;
            }
        } else
        {
            if (Time.time - lastTimeGiat > timeGiat)
            {
                isInElectricShock = false;
                target.OnCheckForAttack -= LockAttack;
                target.OnCheckForMove -= LockMove;
                lastTime = Time.time;
            }
        }
        ThoiGianConLai -= Time.deltaTime;
        if (ThoiGianConLai <= 0)
        {
            EndUp();
        }
    }

    private void LockMove(BoolAction a)
    {
        a.IsOK = false;
    }

    private void LockAttack(BoolAction a)
    {
        a.IsOK = false;
    }

    public override void StartUp(Entity target, float time)
    {
        start = true;
        this.target = target;
        ThoiGianConLai = time;
        isInElectricShock = false;
        lastTimeGiat = Time.time - timeGiat;
        target.OnBuffsChanged?.Invoke(DamageElement.Electric, true);
        target.OnHide += () => Setavalible(true);
        target.OnAppear += () => Setavalible(false);
        target.OnIntoTheGound += () => Setavalible(true);
        target.OnOuttoTheGound += () => Setavalible(false);
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

    private void OnDestroy()
    {
        if (target != null)
        {
            target.OnBuffsChanged?.Invoke(DamageElement.Electric, false);
        }
    }

    private void Setavalible(bool a)
    {
        avalableRender = !a;
    }

    public override void EndUp()
    {
        base.EndUp();
        if (target != null)
        {
            target.OnHide -= () => Setavalible(true);
            target.OnAppear -= () => Setavalible(false);
            target.OnIntoTheGound -= () => Setavalible(true);
            target.OnOuttoTheGound -= () => Setavalible(false);
            target.OnCheckForAttack -= LockAttack;
            target.OnCheckForMove -= LockMove;
        }
    }
}
