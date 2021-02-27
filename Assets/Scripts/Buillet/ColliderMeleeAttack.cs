using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PolygonCollider2D))]
public class ColliderMeleeAttack : MonoBehaviour
{
    [SerializeField] LayerMask TargetLayerAttack;
    [SerializeField] ControlPartice VFXhit;
    [SerializeField] Transform _center;
    List<ITakeHit> listTH = new List<ITakeHit>();
    DamageData damageData;
    bool daming;
    PolygonCollider2D col;
    Vector3 center => (_center == null) ? transform.position : _center.position;

    private PoolingGameObject<ControlPartice> pooling_controlpartice;
    private void Awake()
    {
        if (VFXhit != null)
        {
            pooling_controlpartice = new PoolingGameObject<ControlPartice>(VFXhit);
        }
    }

    public void StartDamage(DamageData damageData)
    {
        daming = true;
        listTH = new List<ITakeHit>();
        this.damageData = damageData;
    }

    public void EndDamage()
    {
        daming = false;
    }

    private void Start()
    {
        col = GetComponent<PolygonCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (daming)
        {
            CheckCollider(collision);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (daming)
        {
            
            CheckCollider(collision);
        }
    }

    private void CheckCollider(Collider2D collision)
    {
        if(TargetLayerAttack == (TargetLayerAttack | 1 << collision.gameObject.layer))
        {
            ITakeHit takeHit = collision.gameObject.GetComponent<ITakeHit>();
            if (takeHit != null && !isExits(takeHit, listTH))
            {
                TakeHit(damageData, takeHit);
            }
        }
    }

    private void CheckCollider(Collision collision)
    {
        if (TargetLayerAttack == (TargetLayerAttack | 1 << collision.gameObject.layer))
        {
            ITakeHit takeHit = collision.gameObject.GetComponent<ITakeHit>();
            if (takeHit != null && !isExits(takeHit, listTH))
            {
                TakeHit(damageData, takeHit);
            }
        }
    }

    private void TakeHit(DamageData damageData, ITakeHit takeHit)
    {
        damageData.Direction = (takeHit.GetCollider().bounds.center - transform.position).normalized;
        takeHit.TakeDamaged(damageData.Clone);
        listTH.Add(takeHit);
        ShowVFX(takeHit.GetCollider());
    }

    private void ShowVFX(Collider2D col)
    {
        if (pooling_controlpartice == null)
            return;
        Vector3 center = this.center;
        Vector3 target = col.bounds.center;
        RaycastHit2D[] raycast = Physics2D.RaycastAll(center, (target - center).normalized, Vector2.Distance(center, target));
        if (raycast != null && raycast.Length != 0)
        {
            foreach (RaycastHit2D ray in raycast)
            {
                if (ray.collider.gameObject == col.gameObject)
                {
                    pooling_controlpartice.Spawn(ray.point, Quaternion.identity).Play();
                }
            }
        }
    }

    private bool isExits(ITakeHit takeHit, List<ITakeHit> takeHits)
    {
        foreach (ITakeHit take in takeHits)
        {
            if (takeHit == take)
            {
                return true;
            }
        }
        return false;
    }
}
