using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Viling : GunBase
{
    [Space]
    [Header("More")]
    [SerializeField] BulletBase bulletExtra;
    [SerializeField] int damageExtra = 4;
    [Tooltip("Số viên đạn phụ bắn ra mỗi bên")]
    [SerializeField] Vector2 RangeAmount = new Vector2 (2, 3);
    [Range(0, 90)]
    [Tooltip("Lệch độ về tâm MIN")]
    [SerializeField] int OffsetA = 20;
    [Range(0, 90)]
    [Tooltip("Lệch độ về tâm MAX")]
    [SerializeField] int OffsetB = 50;


    protected override void Awake()
    {
        base.Awake();
        if (OffsetA > OffsetB)
        {
            int a = OffsetA;
            OffsetA = OffsetB;
            OffsetB = a;
        }
        id_pool_bullet_extra = pool.AddPrefab(bulletExtra);
    }
    private int id_pool_bullet_extra;

    public override void Shoot(DamageData damageData)
    {
        base.Shoot(damageData);
        for (int i = -1; i <= 1; i+=2)
        {
            int am = (int)Random.Range(RangeAmount.x, RangeAmount.y + 1);
            for (int j = 0; j < am; j++) 
            {
                int dolec = i * (Random.Range(OffsetA, OffsetB + 1));
                Vector3 DirShoot = damageData.Direction;
                float z = MathQ.DirectionToRotation(DirShoot).z;
                z += dolec;
                DirShoot = MathQ.RotationToDirection(z);
                SetUpDamageDataExtra(damageData, DirShoot);
                BulletBase bullet = pool.Spawn(id_pool_bullet_extra,PositionStartAttack, Quaternion.identity) as BulletBase;
                bullet.StartUp(damageData);
            }
        }
    }

    protected virtual void SetUpDamageDataExtra(DamageData damageData, Vector3 Direc)
    {
        damageData.Damage = damageExtra;
        damageData.Direction = Direc;
        damageData.FromGunWeapon = true;
        damageData.BackForce = 0;
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawLine(PositionStartAttack, PositionStartAttack + MathQ.RotationToDirection(transform.rotation.eulerAngles.z + OffsetA) * 5f);
        Gizmos.DrawLine(PositionStartAttack, PositionStartAttack + MathQ.RotationToDirection(transform.rotation.eulerAngles.z - OffsetA) * 5f);

        Gizmos.DrawLine(PositionStartAttack, PositionStartAttack + MathQ.RotationToDirection(transform.rotation.eulerAngles.z + OffsetB) * 5f);
        Gizmos.DrawLine(PositionStartAttack, PositionStartAttack + MathQ.RotationToDirection(transform.rotation.eulerAngles.z - OffsetB) * 5f);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        pool.RemovePrefab(id_pool_bullet_extra);
    }
}
