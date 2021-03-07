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

    protected override void FlyAndCheckColloder()
    {
        CheckCollider();
    }

    public override void StartUp(DamageData dam)
    {
        base.StartUp(dam);
        float speed = Random.Range(RangeSpeed.x, RangeSpeed.y);
        iTween.MoveAdd(gameObject, new Vector3(dam.Direction.x * speed, dam.Direction.y * speed, 0), 5f);
    }

    protected override void OnBegin()
    {
        base.OnBegin();
        colorCurrent = Colors.Evaluate(Random.Range(0, 1f));
        render.color = colorCurrent;
    }

    void CheckCollider()
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, RadiusCollider, target);
        if (col != null && col.Length != 0)
        {
            foreach (Collider2D c in col)
            {
                ITakeHit take = c.GetComponent<ITakeHit>();
                if (take != null)
                {
                    OnHitTarget(take, transform.position);
                }
            }
        }
    }

    protected override void OnDestroyed(ControlPartice VFX)
    {
        VFX.SetStartColor(colorCurrent);
        Debug.Log("OK");
        base.OnDestroyed(VFX);
    }



    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, RadiusCollider);
    }


}
