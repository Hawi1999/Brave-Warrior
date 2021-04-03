using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ene_OcSen : EnemyGround
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
            }
            else
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
        float z = MathQ.DirectionToRotation(DirectFire).z;
        for (int i = -1; i <= 1; i++)
        {
            Vector2 dirAttack = MathQ.RotationToDirection(z + OffsetAngleAttack * i);
            BulletEnemy bullet = pool.Spawn(id_bul, PositionSpawnBullet, MathQ.DirectionToQuaternion(dirAttack)) as BulletEnemy;
            DamageData dam = new DamageData();
            SetUpDamageData(dam);
            dam.Direction = dirAttack;
            bullet.StartUp(dam);
        }
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
            }
            else
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
        pool.RemoveAllPooled(id_bul);
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
