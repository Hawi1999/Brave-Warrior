using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamingCircle : MonoBehaviour
{
    public float Radius = 1;
    public int Damage = 4;
    public LayerMask target;
    public float DistanceDaming;
    public bool StartOnAwake;

    private List<TimeToTakeHit> takehits;

    bool started;

    private void Awake()
    {
        started = StartOnAwake;
        takehits = new List<TimeToTakeHit>();
    }

    private void Update()
    {
        
    }
    protected virtual void Daming()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, Radius, target);
        if (cols == null || cols.Length == 0)
        {
            return;
        }
        else
        {
            foreach (Collider2D col in cols)
            {
                ITakeHit take = col.GetComponent<ITakeHit>();
                if (ReadyToDamaged(take))
                {
                    DamageData damage = new DamageData();
                    SetUpDamageData(damage);
                    damage.Direction = Vector3.zero;
                    take.TakeDamaged(damage);
                    takehits.Add(new TimeToTakeHit(take, Time.time));
                }
            }
        }
    }

    private void SetUpDamageData(DamageData damage)
    {
        damage.Damage = this.Damage;
    }

    protected bool ReadyToDamaged(ITakeHit take)
    {

        if (take == null)
        {
            return false;
        }
        TimeToTakeHit tt = Array.Find(takehits.ToArray(), e => e.takeHit == take);
        if (tt == null)
        {
            takehits.Add(new TimeToTakeHit(take, Time.time));
            return true;
        }
        if (Time.time - tt.time > DistanceDaming)
        {
            tt.time = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
