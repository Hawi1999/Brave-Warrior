using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExtraViling : BulletBase
{
    [Header("More")]
    [SerializeField]
    private Vector2 RangeSpeed = new Vector2(2, 5);
    [SerializeField]
    private float _RadiusColldier = 1;
    [SerializeField] Gradient Colors;

    private float RadiusCollider => _RadiusColldier * transform.localScale.x;
    private Color colorCurrent;
    public override void StartUp(DamageData dam)
    {
        base.StartUp(dam);
        float speed = Random.Range(RangeSpeed.x, RangeSpeed.y);
        iTween.MoveAdd(gameObject, new Vector3(dam.Direction.x * speed, dam.Direction.y * speed, 0), 5f);
    }

    protected override void UpdateTransform()
    {
        
    }

    protected override void OnBegin()
    {
        base.OnBegin();
        colorCurrent = Colors.Evaluate(Random.Range(0, 1f));
        render.color = colorCurrent;
    }

    protected override Collider2D[] GetAllCollision()
    {
        return Physics2D.OverlapCircleAll(transform.position, RadiusCollider, target);
    }

    protected override void OnDestroyed(ControlPartice VFX)
    {
        VFX.SetStartColor(colorCurrent);
        base.OnDestroyed(VFX);
    }



    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, RadiusCollider);
    }


}
