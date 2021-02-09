using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class BulletBase : MonoBehaviour
{
    [HideInInspector] protected DamageData damage;
    [SerializeField] protected float flySpeed;
    [SerializeField] private Transform dauDan;
    [SerializeField] protected LayerMask target;
    [SerializeField] GameObject VFXDestroyed;

    SpriteRenderer render => GetComponent<SpriteRenderer>();

    protected Vector3 vitri_daudan
    {
        get
        {
            if (dauDan == null)
            {
                Debug.Log("Chưa hiết lập vị trí đầu đạn, vị trí mặc định là vị trí của viên đạn");
                return transform.position;
            } else
            {
                return dauDan.position;
            }
        }
    }


    private float timeToDestroy = 5;
    private float timelife;

    private void Start()
    {
        timelife = Time.time;
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<PolygonCollider2D>().isTrigger = true;
        }
        render.sortingLayerName = "Current";
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time - timelife > timeToDestroy)
        {
            Destroy(gameObject);
        }
        if (transform.hasChanged)
        {
            render.sortingOrder = (int)(-10f * transform.position.y + 3);
        }
        Fly();
    }

    public virtual void StartUp(DamageData dam)
    {
        damage = dam;
    }



    protected virtual void Fly()
    {
        if (damage.Direction != Vector3.zero)
        {
            Vector2 oldPos = transform.position;
            Vector2 newPos = (transform.position + damage.Direction * flySpeed * Time.deltaTime);
            RaycastHit2D[] hits = Physics2D.RaycastAll(vitri_daudan, damage.Direction, Vector2.Distance(oldPos, newPos), target);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.GetComponent<TakeHit>() != null)
                {
                    OnHitTarget(hit.collider.GetComponent<TakeHit>(), hit);
                }
            }
            transform.position = newPos;
            transform.rotation = MathQ.DirectionToQuaternion(damage.Direction);
        }
    }

    protected virtual void OnHitTarget(TakeHit take, RaycastHit2D hit)
    {
        take.TakeDamaged(damage.Clone);
        Destroyed(hit);
    }

    public void Destroyed(RaycastHit2D hit)
    {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f)));
        if (VFXDestroyed != null)
        {
            Destroy(Instantiate(VFXDestroyed, hit.point, rotation), 0.2f);
        }
        Destroy(gameObject);
    }
}
