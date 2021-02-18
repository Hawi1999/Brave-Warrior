using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_K20 : GunBase
{
    [SerializeField] Transform TFRadanPhaiTren;
    [SerializeField] Transform TFRadanPhaiDuoi;
    [SerializeField] Transform TFRadanPhaiGiua;
    // false cho dưới và 1 cho trên
    private bool vitriTren;
    private int nextAmountbullet = 1;

    public override Vector3 PositionStartAttack
    {
        get
        {
            if (nextAmountbullet == 1)
            {
                return TFRadanPhaiGiua.position;
            }
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

    public override void Shoot(DamageData damageData)
    {
        if (nextAmountbullet == 1)
        {
            Vector3 DirShoot = GiatSung(Host.DirectFire);
            BulletBase bull = Instantiate(VienDan, PositionStartAttack, MathQ.DirectionToQuaternion(DirShoot));
            SetUpDamageData(damageData, DirShoot);
            bull.StartUp(damageData);
            nextAmountbullet = 2;
        } else if (nextAmountbullet == 2)
        {
            vitriTren = true;
            Vector3 DirShoot1 = GiatSung(Host.DirectFire);
            BulletBase bull = Instantiate(VienDan, PositionStartAttack, MathQ.DirectionToQuaternion(DirShoot1));
            SetUpDamageData(damageData, DirShoot1);
            bull.StartUp(damageData);
            vitriTren = false;
            Vector3 DirShoot2 = GiatSung(Host.DirectFire);
            bull = Instantiate(VienDan, PositionStartAttack, MathQ.DirectionToQuaternion(DirShoot2));
            SetUpDamageData(damageData, DirShoot2);
            bull.StartUp(damageData);
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
                if (isLeftDir)
                {
                    Vector3 Do = MathQ.DirectionToRotation(direction);
                    Do += new Vector3(0, 0, Random.Range(-DoGiat / 2, 0));
                    return MathQ.RotationToDirection(Do.z).normalized;
                }
                else
                {
                    Vector3 Do = MathQ.DirectionToRotation(direction);
                    Do += new Vector3(0, 0, Random.Range(0, DoGiat / 2));
                    return MathQ.RotationToDirection(Do.z).normalized;
                }
            }else
            {
                if (isLeftDir)
                {
                    Vector3 Do = MathQ.DirectionToRotation(direction);
                    Do += new Vector3(0, 0, Random.Range(0, DoGiat / 2));
                    return MathQ.RotationToDirection(Do.z).normalized;
                } 
                else
                {
                    Vector3 Do = MathQ.DirectionToRotation(direction);
                    Do += new Vector3(0, 0, Random.Range(-DoGiat/2, 0));
                    return MathQ.RotationToDirection(Do.z).normalized;
                }
            }
        }
    }
}
