using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class ColliderMeleeAttack : MonoBehaviour
{
    [SerializeField] LayerMask TargetLayerAttack;

    List<TakeHit> listTH = new List<TakeHit>();
    DamageData damageData;
    bool daming;

    PolygonCollider2D col;
    public void StartDamage(DamageData damageData)
    {
        daming = true;
        listTH = new List<TakeHit>();
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
            TakeHit takeHit = collision.gameObject.GetComponent<TakeHit>();
            if (takeHit != null && !isExits(takeHit, listTH))
            {
                damageData.Direction = (takeHit.GetCollider().bounds.center - transform.position).normalized;
                takeHit.TakeDamaged(damageData.Clone);
                listTH.Add(takeHit);
            }
        }
    }

    private bool isExits(TakeHit takeHit, List<TakeHit> takeHits)
    {
        foreach (TakeHit take in takeHits)
        {
            if (takeHit == take)
            {
                return true;
            }
        }
        return false;
    }
}
