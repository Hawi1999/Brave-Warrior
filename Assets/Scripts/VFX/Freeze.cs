﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : ElementalBuffBad, ILockMove
{
    float Time_remaining = 0f;

    Entity target;
    SpriteRenderer sprite;
    void Awake()
    {

    }
    public override void StartUp(Entity entity, float time)
    {
        Time_remaining = time;
        target = entity;
        target.OnBuffsChanged?.Invoke(DamageElement.Ice, true);
        SetEvent(true);
        SpawnIce();
    }

    private void Update()
    {
        if (Time_remaining <= 0)
        {
            EndUp();
            return;
        }
        if (sprite != null && target != null)
        {
            sprite.transform.position = target.center;
            sprite.transform.localScale = Vector3.one * Mathf.Max(target.size.x, target.size.y) * 1.5f;
        }
        Time_remaining -= Time.deltaTime;
    }

    private void LockAttack(BoolAction a)
    {
        a.IsOK = false;
    }

    private void LockMove(BoolAction a)
    {
        a.IsOK = false;
    }

    private void SpawnIce()
    {
        if (VFXManager.IcePrefab != null && VFXManager.SpritesIce != null && VFXManager.SpritesIce.Length != 0)
        {
            Sprite[] s = VFXManager.SpritesIce;
            sprite = Instantiate(VFXManager.IcePrefab);
            sprite.sprite = s[Random.Range(0, s.Length)];
        }
    }

    public void AddTime(float time)
    {
        if (time > this.Time_remaining)
        {
            Time_remaining = time;
        }
    }

    public static void Freezed(Entity entity, float time)
    {
        Freeze fre = entity.GetComponent<Freeze>();
        if (!fre)
        {
            fre = entity.gameObject.AddComponent<Freeze>();
            fre.StartUp(entity, time);
        } else
        {
            fre.AddTime(time);
        }
    }
    private void SetEvent(bool a)
    {
        if (target != null)
        {
            if (a)
            {
                target.OnCheckForAttack += LockAttack;
                target.OnCheckForMove += LockMove;
                target.OnDeath += (Enemy) => OnEntityDead();
            } else
            {
                target.OnCheckForAttack -= LockAttack;
                target.OnCheckForMove -= LockMove;
                target.OnDeath -= (Enemy) => OnEntityDead();
            }
        }
    }

    private void OnEntityDead()
    {
        EndUp();
    }

    public void EndLockMove()
    {
        EndUp();
    }

    public override void EndUp()
    {
        if (sprite != null)
        {
            Destroy(sprite.gameObject);
        }
        if (target != null)
        {
            target.OnBuffsChanged?.Invoke(DamageElement.Ice, false);
            SetEvent(false);
        }
        base.EndUp();
    }
}
