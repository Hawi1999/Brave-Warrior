using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class BulletBase : PoolingBehaviour
{
    [HideInInspector] protected DamageData damage;
    [SerializeField] protected float flySpeed;
    [SerializeField] private Transform dauDan;
    [SerializeField] protected LayerMask target;
    [SerializeField] ControlPartice VFXDestroyed;

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

    PoolingGameObject<ControlPartice> pooling_vfx;
    private void Awake()
    {
        if (VFXDestroyed != null)
        {
            pooling_vfx = new PoolingGameObject<ControlPartice>(VFXDestroyed);
        }
    }
    protected virtual void Start()
    {
        timelife = Time.time;
        render.sortingLayerName = "Current";
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time - timelife > timeToDestroy)
        {
            Rest();
        }
        if (transform.hasChanged)
        {
            render.sortingOrder = (int)(-10f * transform.position.y + 3);
        }
        Fly();
    }

    protected override void OnBegin()
    {
        base.OnBegin();
        timelife = Time.time;
    }

    public virtual void StartUp(DamageData dam)
    {
        damage = dam.Clone;
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
                if (hit.collider != null && hit.collider.GetComponent<ITakeHit>() != null)
                {
                    damage.PointHit = hit.point;
                    OnHitTarget(hit.collider.GetComponent<ITakeHit>(), damage.PointHit);
                }
            }
            transform.position = newPos;
            transform.rotation = MathQ.DirectionToQuaternion(damage.Direction);
        }
    }

    protected virtual void OnHitTarget(ITakeHit take, Vector3 point)
    {
        take.TakeDamaged(damage.Clone);
        Destroyed(damage.PointHit);
    }


    public void Destroyed(Vector3 position)
    {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f)));
        if (VFXDestroyed != null)
        {
            pooling_vfx.Spawn(position, rotation).Play();
        }
        Rest();
    }

    public void Destroyed()
    {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f)));
        if (VFXDestroyed != null)
        {
            pooling_vfx.Spawn(transform.position, rotation).Play();
        }
        Rest();
    }

    private void OnDestroy()
    {
        pooling_vfx?.DestroyAll();
    }
}
