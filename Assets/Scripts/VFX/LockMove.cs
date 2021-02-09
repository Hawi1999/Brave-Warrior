﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMove : MonoBehaviour, ILockMove
{
    bool start = false;
    float time = 0;
    Entity entity;
    private void Update()
    {
        if (start)
        {
            if (time <= 0){
                entity.OnCheckForMove -= Lock;
                Destroy(this);
            }
            time -= Time.deltaTime;
        }
    }

    public void SetStart(Entity entity, float time)
    {
        start = true;
        this.time = time;
        this.entity = entity;
        entity.OnCheckForMove += Lock;
    }

    public void SetTime(float Time)
    {
        if (Time > time)
        {
            time = Time;
        }
    }

    public static void LockByTime(Entity entity, float time)
    {
        LockMove Lock = entity.GetComponent<LockMove>();
        if (Lock == null)
        {
            Lock = entity.gameObject.AddComponent<LockMove>();
            Lock.SetStart(entity, time);
        } else
        {
            Lock.SetTime(time);
        }
    }

    private void Lock(BoolAction permit)
    {
        permit.IsOK = false;
    }

    private void OnDestroy()
    {
        entity.OnCheckForMove -= Lock;
        Debug.Log(Time.time);
    }

    public void EndLockMove()
    {
        Destroy(this);
    }
}