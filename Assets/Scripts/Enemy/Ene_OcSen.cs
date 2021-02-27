using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ene_OcSen : Enemy
{
    protected override void Attack()
    {
        Vector2 dirAttack = getRotationToPlayer(((Vector2)targetAttack.ColliderTakeDamaged.bounds.center - vitriradan).normalized);
        for (int i = -1; i <= 1; i++)
        {
            Vector2 NewDirection = ChangeDiretion(dirAttack, 45 * i);
            BulletBase bull = Instantiate(ED.BulletPrefabs, vitriradan, MathQ.DirectionToQuaternion(NewDirection));
            DamageData dam = setUpDamageData(NewDirection);
            bull.StartUp(dam);
        }
    }


    private Vector2 ChangeDiretion(Vector2 goc, float off)
    {
        Vector3 rotation = MathQ.DirectionToRotation(goc);
        rotation.z += off;
        return MathQ.RotationToDirection(rotation.z).normalized;
    }

    public override Vector3 getPosition()
    {
        return transform.position + new Vector3(0, 0.15f, 0);
    }

    public override Vector2 center => transform.position + new Vector3(0, 0.255f);
}
