using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFollow : BulletBase
{
    [SerializeField] float SpeedRotate = 0.1f;

    private IFindTarget follow;
    protected override void UpdateTransform()
    {
        UpdateDirection();
        base.UpdateTransform();
    }

    void UpdateDirection()
    {
        if (follow != null && follow as UnityEngine.Object != null)
        {
            Vector3 targetFollow = follow.center;
            Vector2 Direct2 = (targetFollow - transform.position).normalized;
            float zx = MathQ.DirectionToRotation(Direct2).z;
            float zc = MathQ.DirectionToRotation(damage.Direction).z;
            float zz;
            float z = SpeedRotate * 360 * Time.deltaTime;
            if (!isLeft(damage.Direction, follow.center))
            {
                zz = zc - z;
            }
            else
            {
                zz = zc + z;
            }
            if ((zz - zx) * (zc - zx) <= 0)
            {
                zz = zx;
            }
            damage.Direction = MathQ.RotationToDirection(zz);
        }
    }


    bool isLeft(Vector2 A, Vector2 posCheck)
    {
        Vector2 B = (posCheck - (Vector2)transform.position).normalized;
        return -A.x * B.y + A.y * B.x < 0;
    }

    public void SetTarget(IFindTarget transform)
    {
        follow = transform;
    }
}
