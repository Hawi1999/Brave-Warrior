using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class BulletBase : PoolingBehaviour
{
    [HideInInspector] protected DamageData damage;
    [SerializeField] protected float flySpeed = 15;
    [SerializeField] private Transform dauDan;
    [SerializeField] protected LayerMask target;
    [SerializeField] private float timeToDestroy = 5;
    [SerializeField] protected ControlPartice VFXDestroyed;

    protected SpriteRenderer render;
    [HideInInspector] public bool isEnable = true;

    protected Vector3 vitri_daudan
    {
        get
        {
            if (dauDan == null)
            {
                return transform.position;
            } else
            {
                return dauDan.position;
            }
        }
    }
    private float timelife;

    private PoolingGameObject<ControlPartice> pooling_vfx;
    protected virtual void Awake()
    {
        if (VFXDestroyed != null)
        {
            pooling_vfx = new PoolingGameObject<ControlPartice>(VFXDestroyed);
        }render = GetComponent<SpriteRenderer>();
        render.sortingLayerName = "Current";
        gameObject.layer = LayerMask.NameToLayer("Bullet");
    }
    protected virtual void Start()
    {
        timelife = Time.time;
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isEnable)
            return;
        if (Time.time - timelife > timeToDestroy)
        {
            Destroyed();
        }
        if (transform.hasChanged)
        {
            render.sortingOrder = (int)(-10f * transform.position.y + 3);
        }
        FlyAndCheckColloder();
    }

    protected override void OnBegin()
    {
        base.OnBegin();
        isEnable = true;
        timelife = Time.time;
    }

    public virtual void StartUp(DamageData dam)
    {
        damage = dam.Clone;
    }


    protected virtual void FlyAndCheckColloder()
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
                    OnHitTarget(hit.collider.GetComponent<ITakeHit>(), hit.point);
                }
            }
            transform.position = newPos;
            transform.rotation = MathQ.DirectionToQuaternion(damage.Direction);
        }
    }

    protected virtual void OnHitTarget(ITakeHit take, Vector3 point)
    {
        DamageData da = damage.Clone;
        da.PointHit = point;
        take.TakeDamaged(da);
        Destroyed(point);
    }


    public void Destroyed(Vector3 position)
    {
        if (VFXDestroyed != null)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f)));
            OnDestroyed(pooling_vfx.Spawn(position, rotation));
        }
        isEnable = false;
        OnAfterDestroyed();
    }

    public void Destroyed()
    {
        if (VFXDestroyed != null)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f)));
            OnDestroyed(pooling_vfx.Spawn(transform.position, rotation));
        }
        isEnable = false;
        OnAfterDestroyed();
    }

    protected virtual void OnAfterDestroyed()
    {
        Rest();
    }

    protected virtual void OnDestroyed(ControlPartice VFX)
    {
        VFX.Play();
    }

    protected virtual void OnDestroy()
    {
        pooling_vfx?.DestroyAll();
    }

    protected virtual void OnDrawGizmos()
    {

    }
}
