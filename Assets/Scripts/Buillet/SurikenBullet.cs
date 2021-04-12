using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurikenBullet : BulletBase
{
    [SerializeField] float _radius = 0.5f;
    [SerializeField] float _speedRotation = 1f;
    protected override Collider2D[] GetAllCollision()
    {
        return Physics2D.OverlapCircleAll(transform.position, _radius, target);
    }

    protected override void OnBegin()
    {
        base.OnBegin();
        _speedRotation *= Random.Range(0, 2) == 1 ? 1 : -1;
    }

    protected override void UpdateRotation()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + _speedRotation * 360 * Time.deltaTime));
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
