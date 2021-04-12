using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ene_BlueSlime : EnemyGround
{
    [SerializeField] BulletBase bulletPrefabs;
    [SerializeField] Transform OffSetSpawnBullet;
    [SerializeField] int Damage = 4;
    [SerializeField] float timeAttack = 1;

    private float DistancePositonSpawnBull
    {
        get
        {
            if (OffSetSpawnBullet == null)
            {
                return 0;
            }
            return Vector2.Distance(OffSetSpawnBullet.position, center);
        }
    }

    private int id_bul;

    protected override void SetUpAwake()
    {
        base.SetUpAwake();
        id_bul = pool.AddPrefab(bulletPrefabs);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        pool.RemovePrefab(id_bul);
    }

    protected override void ChooseNextAction()
    {
        if (CurrentAction == Action.Idle)
        {
            if (Random.Range(0, 4) > 1 && PermitAttack)
            {
                SetNewAction(Action.Attack);
                OnBeginAttack();
            } else
            {
                SetNewAction(Action.Move);
                OnBeginMove();
            }
        }
    }

    protected override void UpdateAttack()
    {
        if (WaitToChooseNextAction)
        {
            SetNewAction(Action.Idle);
            OnBeginIdle();
        } else
        {
            Attacking();
            CheckTimeToNextAction();
        }
}

    #region Attack
    bool attacked = false;
    private void OnBeginAttack()
    {
        if (!PermitAttack)
        {
            SetNewAction(Action.Idle);
            OnBeginIdle();
            return;
        }
        attacked = false;
        SetTimeToNextAction(timeAttack);
        SetAnimation(Animate_Attack);
    }
    private void Attacking()
    {
        if (!attacked && Time.time - time_start_action > 7 * time_action / 12)
        {
            float z = Random.Range(0, 60);
            for (int i = 0; i < 6; i++)
            {
                float zz = z + 60 * i;
                Vector2 dirAttack = MathQ.RotationToDirection(zz);
                Vector2 vitriradan = center + dirAttack * DistancePositonSpawnBull;
                BulletEnemy bullet = pool.Spawn(id_bul,vitriradan, MathQ.DirectionToQuaternion(dirAttack)) as BulletEnemy;
                DamageData dam = new DamageData();
                SetUpDamageData(dam);
                dam.Direction = dirAttack;
                bullet.StartUp(dam);
            }
            attacked = true;
        }
    }

    protected override void SetUpDamageData(DamageData dam)
    {
        base.SetUpDamageData(dam);
        dam.Damage = this.Damage;
        dam.Type = DamageElement.Ice;
        dam.IceTime = 1;
        dam.IceRatio = 0.7f;
    }


    #endregion

    protected override void XLDIce(DamageData damaData)
    {
        damaData.AddDecreaseByPercent(0.5f);
    }

    protected override void XLDFireFireFrom(DamageData damageData)
    {
        base.XLDFireFireFrom(damageData);
        damageData.AddDecreaseByPercent(-2);
    }
}
