using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TNTTakeHit : MonoBehaviour, ITakeHit
{
    public int Damage = 25;
    public float BackFore = 2f;
    public float Radius = 2;
    public LayerMask layerTarget;
    public Sprite[] sprites;
    public int FPS;
    
    bool Booming;
    bool damaged;
    DamageData DamageData;
    int id = -1;
    float tim;


    private void Update()
    {
        if (Booming)
        {
            if ((sprites == null || sprites.Length == 0) && !damaged)
            {
                Damaged();
                damaged = true;
                Destroy(gameObject);
                return;
            }
            tim += Time.deltaTime;
            id = (int)(tim / (1f/FPS));
            if (id >= sprites.Length/2)
            {
                if (!damaged)
                {
                    Damaged();
                    damaged = true;
                }
            }
            if (id >= sprites.Length)
            {
                Destroy(gameObject);
            } else
            {
                GetComponent<SpriteRenderer>().sprite = sprites[id];
            }
        }
    }
    public void TakeDamaged(DamageData data)
    {
        if (!Booming)
        {
            DamageData = data;
            Booming = true;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f)));
            GetComponent<Collider2D>().enabled = false;
        }
        
    }

    private void Damaged()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, Radius, layerTarget);
        if (collider2Ds == null || collider2Ds.Length == 0)
        {
            return;
        }
        foreach (Collider2D collider2D in collider2Ds)
        {
            DamageData damage = DamageData.Clone;
            damage.Type = DamageElement.Fire;
            ITakeHit take = collider2D.gameObject.GetComponent<ITakeHit>();
            if (take != null)
            {
                float Distance = Vector2.Distance(take.GetCollider().bounds.center,transform.position);
                Distance = Mathf.Clamp(Distance, 0f, Radius);
                damage.Damage = (int)(((float)((Radius - Distance)/Radius)) * Damage);
                damage.BackForce = ((Radius - Distance)/(Radius)) * BackFore;
                damage.Direction = (take.GetCollider().bounds.center - transform.position).normalized;
                damage.FireRatio = 1f;
                damage.FromTNT = true;
                take.TakeDamaged(damage);
            }
        }
    }


    public Collider2D GetCollider()
    {
        return GetComponent<Collider2D>();
    }
}
