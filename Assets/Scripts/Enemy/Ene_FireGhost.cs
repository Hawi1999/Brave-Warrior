using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ene_FireGhost : EnemyGhost
{
    [SerializeField] BulletFollow Bullet;
    [SerializeField] int Damage;
    [SerializeField] Transform SpawnBullet;
    [SerializeField] LayerMask targetFolow;
    Vector3 positionSpawnBullet
    {
        get
        {
            if (SpawnBullet == null)
            {
                return center;
            }
            return SpawnBullet.position;
        }
    }


    int id_bull;
    protected override void Awake()
    {
        base.Awake();
        id_bull = pool.AddPrefab(Bullet);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        pool.RemovePrefab(id_bull);
    }
    protected override void ChooseNextAction()
    {
        if (CurrentAction == Action.Idle)
        {
            int a = Random.Range(0, 5);
            if (a > 1 && PermitAttack)
            {
                SetNewAction(Action.Attack);
                OnBeginAttack();
                
            } else if (a == 1)
            {
                SetNewAction(Action.StartHide);
                OnBeginStartHide();
            } else
            {
                SetNewAction(Action.Move);
                OnBeginMove();
            }
            return;
        }
        if (CurrentAction == Action.Attack)
        {
            SetNewAction(Action.Idle);
            OnBeginIdle();
        }
    }
    #region Attack

    float timeAttack1 = 1.5f;
    float timeAttack2 = 1f;

    float time_delay_every = 0.4f;
    float time_next_attack;

    bool attack1;
    bool attack2;

    int maxamount = 0;
    int amountAttack = 0;
    protected override void UpdateAttack()
    {
        if (WaitToChooseNextAction)
        {
            OnEndAttack();
            ChooseNextAction();
        } else
        {
            Attacking();
            CheckTimeToNextAction();
        }
    }

    private void OnBeginAttack()
    {
        if (Random.Range(0, 1f) >= 0.5f)
        {
            attack1 = true;
            SetTimeToNextAction(timeAttack1);
            time_next_attack = Time.time + timeAttack1 / 2 - time_delay_every;
            maxamount = 3;
        }
        else
        {
            attack2 = true;
            SetTimeToNextAction(timeAttack2);
            time_next_attack = Time.time + timeAttack2 / 2 - time_delay_every;
            maxamount = 2;
        }
        amountAttack = 0;
    }

    private void Attacking()
    {
        if (Time.time >= time_next_attack && amountAttack < maxamount)
        {
            amountAttack++;
            time_next_attack += time_delay_every;
            if (attack1)
            {
                Attacking1();
            }
            if (attack2)
            {
                Attacking2();
            }
        }
    }

    private void Attacking1()
    {
        float startAngle = MathQ.DirectionToRotation(DirectFire).z - 20 * (amountAttack);
        startAngle = startAngle < -180 ? startAngle + 360 : startAngle;
        for (int i = 0; i < amountAttack; i++)
        {
            float angle = startAngle + i * 40;
            if (angle >= 180)
            {
                angle -= 360;
            }
            Vector3 Direction = MathQ.RotationToDirection(angle);
            DamageData damage = new DamageData();
            SetUpDamageData(damage);
            damage.Direction = Direction;
            BulletFollow bul = pool.Spawn(id_bull, positionSpawnBullet,MathQ.DirectionToQuaternion(Direction)) as BulletFollow;
            bul.SetTarget(Find.FindTargetNearest(center, 7f, targetFolow));
            bul.StartUp(damage);
        }
    }

    private void Attacking2()
    {
        float startAngle = MathQ.DirectionToRotation(DirectFire).z - 20;
        startAngle = startAngle < -180 ? startAngle + 360 : startAngle;
        for (int i = 0; i < 2; i++)
        {
            float angle = startAngle + i * 20;
            if (angle >= 180)
            {
                angle -= 360;
            }
            Vector3 Direction = MathQ.RotationToDirection(angle);
            DamageData damage = new DamageData();
            SetUpDamageData(damage);
            damage.Direction = Direction;
            BulletFollow bul = pool.Spawn(id_bull, positionSpawnBullet, MathQ.DirectionToQuaternion(Direction)) as BulletFollow;
            bul.SetTarget(Find.FindTargetNearest(center, 7f, targetFolow));
            bul.StartUp(damage);
        }
    }

    protected override void SetUpDamageData(DamageData damage)
    {
        base.SetUpDamageData(damage);
        damage.Damage = Damage;
        damage.Type = DamageElement.Fire;
        damage.FireRatio = 0.2f;
        damage.FireTime = 2f;
    }


    private void OnEndAttack()
    {
        attack1 = false;
        attack2 = false;
    }

    #endregion

    protected override void XLDFireNotFireFrom(DamageData damageData)
    {
        damageData.AddText(ShowText.StringColor(Languages.getString("MienNhiem"), "red"));
    }

    protected override void XLDIce(DamageData damaData)
    {
        damaData.AddText(ShowText.StringColor(Languages.getString("MienNhiem"), "blue"));
    }

}

