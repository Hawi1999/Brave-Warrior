using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_K20 : GunBase
{
    [SerializeField] Transform TFRadanPhaiTren;
    [SerializeField] Transform TFRadanPhaiDuoi;

    // false cho dưới và 1 cho trên
    private bool vitriTren;
    private int nextAmountbullet = 1;
    public override Vector3 viTriRaDan
    {
        get
        {
            if (render.flipY)
            {
                if (vitriTren)
                {
                    return TFRadanPhaiDuoi.position;
                }
                else
                {
                    return TFRadanPhaiTren.position;
                }
            }
            else
            {
                if (vitriTren)
                {
                    return TFRadanPhaiTren.position;
                }
                else
                {
                    return TFRadanPhaiDuoi.position;
                }
            }
        }
    }

    public override void Shoot()
    {
        if (!ReadyToAttack)
            return;
        if (nextAmountbullet == 1)
        {
            Vector3 DirShoot = GiatSung(Host.DirectFire);
            BulletBase bull = Instantiate(VienDan, viTriRaDan, MathQ.DirectionToQuaternion(DirShoot));
            bull.StartUp(Host, DirShoot.normalized, new DamageData(SatThuong, DirShoot, default, Host, new RaycastHit2D()));
            lastShoot = Time.time;
            nextAmountbullet = 2;
        } else if (nextAmountbullet == 2)
        {
            vitriTren = true;
            Vector3 DirShoot = GiatSung(Host.DirectFire);
            BulletBase bull = Instantiate(VienDan, viTriRaDan, MathQ.DirectionToQuaternion(DirShoot));
            bull.StartUp(Host, DirShoot.normalized, new DamageData(SatThuong, DirShoot, default, Host, new RaycastHit2D()));

            vitriTren = false;
            DirShoot = GiatSung(Host.DirectFire);
            bull = Instantiate(VienDan, viTriRaDan, MathQ.DirectionToQuaternion(DirShoot));
            bull.StartUp(Host, DirShoot.normalized, new DamageData(SatThuong, DirShoot, default, Host, new RaycastHit2D()));
            lastShoot = Time.time;
            nextAmountbullet = 1;
        }
    }

    protected override Vector3 GiatSung(Vector3 direction)
    {
        if (nextAmountbullet == 1)
            return base.GiatSung(direction);
        else 
        {
            if (vitriTren)
            {
                Vector3 Do = MathQ.DirectionToRotation(direction);
                Do += new Vector3(0, 0, Random.Range(0, DoGiat / 2));
                return MathQ.RotationToDirection(Do.z).normalized;
            }else
            {
                Vector3 Do = MathQ.DirectionToRotation(direction);
                Do += new Vector3(0, 0, Random.Range(-DoGiat/2, 0));
                return MathQ.RotationToDirection(Do.z).normalized;
            }
        }
    }
}
