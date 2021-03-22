using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GunLazeJerky))]
public class GunLaze : Weapon
{
    public Lazer laser;
    public float speedShoot = 2;
    [Range(0, 1)]
    public float CriticalRate = 0.2f;

    public override Vector3 PositionStartAttack => laser.laserFirePoint.position;

    protected override bool ReadyToAttack => true;

    private bool isLeftDir = false;
    private float DistanceDamaged
    {
        get
        {
            if (speedShoot == 0)
            {
                return 0.5f;
            } else
            {
                return 1 / speedShoot;
            }
        }
    }
    private GunLazeJerky jerky;
    private List<TimeToTakeHit> takehits = new List<TimeToTakeHit>();
    protected virtual bool ReadyToDamaged(ITakeHit take)
    {
        if (take == null)
        {
            return false;
        }
        TimeToTakeHit tt = Array.Find(takehits.ToArray(), e => e.takeHit == take);
        if (tt == null)
        {
            takehits.Add(new TimeToTakeHit(take, Time.time));
            return true;
        }
        if (Time.time - tt.time > DistanceDamaged)
        {
            tt.time = Time.time;
            return true;
        } else
        {
            return false;
        }
    }
    public override bool Attack(DamageData damageData)
    {
        if (ReadyToAttack)
        {
            Shoot(damageData);
            return true;
        } else
        {
            return false;
        }
    }

    public override string GetNameOfWeapon()
    {
        return "Gun " + nameOfWeapon;
    }

    protected override void Awake()
    {
        base.Awake();
        jerky = GetComponent<GunLazeJerky>();
    }

    protected void Update()
    {
        RotationGun();
    }


    private void RotationGun()
    {
        if (Host == null)
            return;
        transform.rotation = MathQ.DirectionToQuaternion(Host.DirectFire);
        isLeftDir = Host.DirectFire.x < 0;
        render.flipY = isLeftDir;
    }

    protected virtual void Shoot(DamageData damage)
    {
        laser.Lit(out RaycastHit2D ray, out bool has);
        if (!has)
        {
            return;
        }
        DamageData damageData = damage.Clone;
        if (ray.collider == null)
        {
            return;
        }
        ITakeHit takehit = ray.collider.GetComponent<ITakeHit>();
        if (ReadyToDamaged(takehit))
        {
            SetUpDamageData(damageData);
            takehit.TakeDamaged(damageData);
        }
    }

    protected virtual void SetUpDamageData(DamageData damage)
    {
        bool isCritical = UnityEngine.Random.Range(0, 1f) < CriticalRate;
        int SatThuong = this.SatThuong;
        if (isCritical) SatThuong = (int)(this.SatThuong * (this.CriticalRate + 1));
        damage.Damage = SatThuong;
        damage.Direction = Host.DirectFire;
        damage.BackForce = 0;
        damage.IsCritical = isCritical;
    }

    public override void OnEquip()
    {
        if (laser != null)
        {
            Host.OnNotAttack += jerky.OnNotAttack;
            Host.OnAttacked += jerky.OnAttacked;
            Host.OnNotAttack += laser.UnLit;
            jerky.SetLocalPostionBegin();
        }
    }

    public override void OnTuDo()
    {
        if (laser != null)
        {
            if (lastHost != null)
            {
                lastHost.OnNotAttack -= laser.UnLit;
                lastHost.OnNotAttack -= jerky.OnNotAttack;
                lastHost.OnAttacked -= jerky.OnAttacked;
            }
        }
    }

    public override float TakeTied => Time.deltaTime * 0.5f;

    public override void reset()
    {
        base.reset();
        jerky.reset();
        laser.UnLit();
    }
}
