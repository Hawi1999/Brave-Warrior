using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public float Radius = 2;
    public int timeExplose = 4;
    public int timeDelay = 0;
    public LayerMask layerTarget;
    public int Damage = 10;
    [SerializeField]
    SpriteRenderer renderBoom;
    [SerializeField]
    Animator aniRenderBoom;
    [SerializeField] GameObject PRBoom;
    [SerializeField] GameObject PRVFX;

    DamageData DamageData;
    private void Awake()
    {
        PRVFX.SetActive(false);
    }

    private void Start()
    {
        aniRenderBoom.speed = 5 / (timeExplose + timeDelay);
        Invoke("StartTimer", timeDelay);
    }

    private void Explose()
    {
        PRBoom.SetActive(false);
        renderBoom.enabled = false;
        PRVFX.SetActive(true);
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, Radius, layerTarget);
        if (cols != null && cols.Length != 0)
        {
            foreach (Collider2D col in cols)
            {
                if (col.TryGetComponent(out ITakeHit take))
                {
                    DamageData damage = this.DamageData.Clone;
                    Vector3 target = take.GetCollider().bounds.center;
                    damage.Damage = (int)(this.Damage * Mathf.Clamp01((Radius - (Vector2.Distance(target, transform.position))) / Radius));
                    damage.BackForce = 5 * Mathf.Clamp01((Radius - (Vector2.Distance(target, transform.position))) / Radius) * Radius;
                    damage.Direction = (target - transform.position).normalized;
                    take.TakeDamaged(damage);
                }
            }
        }
        Invoke("DestroyThis", 3f);
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }

    public void StartTimer()
    {
        Invoke("Explose", timeExplose);
    }
    public void SetUp(DamageData damage)
    {
        this.DamageData = damage;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }



}
