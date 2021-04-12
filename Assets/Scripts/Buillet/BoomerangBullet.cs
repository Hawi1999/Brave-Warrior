using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBullet : BulletBase
{
    List<Collider2D> attaked = new List<Collider2D>();
    [SerializeField] float maxDistance = 10;
    [SerializeField] float _radius = 1;
    [SerializeField] float _speedRotate = 1f;
    float MaxDistance => maxDistance;

    float CurrentDistance = 0;

    bool going = false;
    protected override void OnBegin()
    {
        base.OnBegin();
        CurrentDistance = 0;
        going = true;
        _speedRotate *= Random.Range(0, 2) == 1 ? 1 : -1;
        attaked.Clear();
    }
    protected override void OnHitTarget(ITakeHit take, Vector3 point)
    {
        DamageData da = damage.Clone;
        da.PointHit = point;
        attaked.Add(take.GetCollider());
        take.TakeDamaged(da);
        if (VFXDestroyed != null)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f)));
            OnDestroyed(pool.Spawn(id_pooling_vfx, transform.position, rotation) as ControlPartice);
        }
    }
    protected override void UpdateRotation()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + _speedRotate * 360 * Time.deltaTime));
    }

    protected override Collider2D[] GetAllCollision()
    {
        Collider2D[] a = Physics2D.OverlapCircleAll(transform.position, _radius, target);
        List<Collider2D> b = new List<Collider2D>();
        for (int i = 0; i < a.Length; i++)
        {
            if (!attaked.Contains(a[i]))
            {
                b.Add(a[i]);
            }
        }
        return b.ToArray();
    }

    protected override void Update()
    {
        if (going)
        {
            CurrentDistance += (oldposition - newposition).magnitude;
            if (CurrentDistance >= MaxDistance)
            {
                attaked.Clear();
                going = false;
            }
        } else
        {
            Entity target = damage.From;
            if (Vector3.Distance(transform.position, target.center) <= 0.2f)
            {
                Rest();
                return;
            }
            damage.Direction = (target.center - (Vector2)transform.position).normalized;
        }
        base.Update();
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
