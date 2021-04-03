using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ene_Sau : EnemyGround
{
    [SerializeField] int SatThuong;
    [SerializeField] BulletBase bulletPrefab;
    [SerializeField] Transform _PositionSpawnBullet;
    [Range(0, 90)]
    [SerializeField] int OffsetAngleAttack = 30;

    private int id_bul;
    private Vector2 PositionSpawnBullet
    {
        get
        {
            if (_PositionSpawnBullet == null)
            {
                return transform.position;
            } else
            {
                return _PositionSpawnBullet.position;
            }
        }
    }

    #region StartAndUpdate
    protected override void Awake()
    {
        base.Awake();
        id_bul = pool.AddPrefab(bulletPrefab);
    }

    #endregion

    #region Tấn công

    protected override void UpdateAttack()
    {
        if (WaitToChooseNextAction)
        {
            EndAttacking();
            SetNewAction(Action.Idle);
            OnBeginIdle();
        }
    }

    protected virtual void OnBeginAttack()
    {
        Vector2 dirAttack = getRotationToPlayer((TargetFire.center - PositionSpawnBullet).normalized, OffsetAngleAttack);
        BulletEnemy bullet = pool.Spawn(id_bul, PositionSpawnBullet, MathQ.DirectionToQuaternion(dirAttack)) as BulletEnemy;
        DamageData dam = new DamageData();
        SetUpDamageData(dam);
        dam.Direction = dirAttack;
        bullet.StartUp(dam);
        SetNewAction(Action.Idle);
        OnBeginIdle();
    }
    protected virtual void Attacking()
    {
    }

    protected virtual void EndAttacking()
    {

    }

    protected override void SetUpDamageData(DamageData damage)
    {
        base.SetUpDamageData(damage);
        damage.Damage = SatThuong;
    }
    #endregion

    #region Chung
    protected override void ChooseNextAction()
    {
        if (CurrentAction == Action.Idle)
        {
            int a = Random.Range(0, 4);
            if (a > 1 && PermitAttack)
            {
                SetNewAction(Action.Attack);
                OnBeginAttack();
            } else
            {
                SetNewAction(Action.Move);
                OnBeginMove();
            }
            return;
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        pool.RemovePrefab(id_bul);
    }

    protected override void OnDead()
    {
        base.OnDead();
        pool.RemovePrefab(id_bul);
    }

    public override void Revive()
    {
        base.Revive();
        id_bul = pool.AddPrefab(bulletPrefab);
    }

    #endregion

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Vector2 dir = MathQ.RotationToDirection(OffsetAngleAttack);
        Gizmos.DrawLine(PositionSpawnBullet, PositionSpawnBullet + dir * 10);
        dir = MathQ.RotationToDirection(-OffsetAngleAttack);
        Gizmos.DrawLine(PositionSpawnBullet, PositionSpawnBullet + dir * 10);
    }
}
