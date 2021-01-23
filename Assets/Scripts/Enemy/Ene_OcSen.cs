using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ene_OcSen : Enemy
{
    protected override void Attack()
    {
        Vector2 dirAttack = getDirToPlayer(((Vector2)targetAttack.ColliderTakeDamaged.bounds.center - vitriradan).normalized);
        for (int i = -1; i <= 1; i++)
        {
            Vector2 NewDirection = ChangeDiretion(dirAttack, 45 * i);
            Instantiate(ED.BulletPrefabs, vitriradan, MathQ.DirectionToQuaternion(NewDirection)).StartUp(NewDirection);
        }
    }

    private Vector2 ChangeDiretion(Vector2 goc, float off)
    {
        Vector3 rotation = MathQ.DirectionToRotation(goc);
        rotation.z += off;
        return MathQ.RotationToDirection(rotation.z).normalized;
    }
}
