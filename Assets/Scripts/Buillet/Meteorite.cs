using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Meteorite : PoolingBehaviour
{
    [SerializeField] string Path = "Sprites/Meteorites";
    [SerializeField] LayerMask Target;
    [SerializeField] SpriteRenderer render;
    [SerializeField] int Damage = 5;
    [SerializeField] Transform Head;
    [SerializeField] float ScaleDefault = 0.5f;
    [SerializeField] float ScalePerLevel = 0.3f;
    [SerializeField] float HpPerLevel = 10;
    private int levelScale;
    private int hp;

    private float DirectRotate;
    private Vector2 DirectMove;
    private float speedAdd = 1;
    private float speedDefault = 5;
    private float maxScale;
    Sprite[] sp;
    private float Speed => speedAdd * speedDefault;

    private Vector2 head => Head == null ? transform.position : Head.position;

    public Vector2 size => Vector2.one * levelScale * ScaleDefault * ScalePerLevel;


    private void Awake()
    {
        sp = Resources.LoadAll<Sprite>(Path);
    }

    protected override void OnBegin()
    {
        base.OnBegin();
        if (sp == null || sp.Length == 0)
        {
            Debug.Log("Sprite Meteorites are null");
            render.sprite = null;
        } else
        {
            render.sprite = sp[Random.Range(0, sp.Length)];
        }
        if (Random.Range(0, 2) == 1)
        {
            DirectRotate = 45;
        } else
        {
            DirectRotate = -45;
        }
        speedDefault = Random.Range(0.5f, 1);
    }

    private void Update()
    {
        float z = render.transform.rotation.eulerAngles.z;
        z += DirectRotate * Time.deltaTime * Speed;
        render.transform.rotation = Quaternion.Euler(new Vector3(0, 0, z));

        transform.position = transform.position + (Vector3)(DirectMove * Time.deltaTime * Speed);
        CheckCollision(Physics2D.OverlapCircleAll(transform.position, size.magnitude / 3, Target));
        render.sortingOrder = (int)(-10f * (transform.position.y));
    }

    private void CheckCollision(Collider2D[] rays)
    {
        if (rays == null || rays.Length == 0)
        {
            return;
        }
        bool has = false;
        foreach (Collider2D ray in rays)
        {
            if (ray.gameObject.TryGetComponent(out ITakeHit take))
            {
                DamageData damage = new DamageData();
                damage.Damage = (int)(this.Damage * (levelScale / maxScale));
                damage.Direction = (take.GetCollider().bounds.center - transform.position).normalized;
                take.TakeDamaged(damage);
                has = true;
            }
        }
        if (has)
        {
            Destroy();
        }
    }
    public void TakeDamage(DamageData damage)
    {
        hp -= (damage.Damage + 5) / 5;
        OnHit?.Invoke(damage);
        if (hp <= 0)
        {
            Rest();
            return;
        }
        while (HpPerLevel * (levelScale - 1) > hp)
        {
            levelScale--;
            UpdateScale();
        }
    }

    private void UpdateScale()
    {
        if (sp != null && sp.Length != 0)
        {
            render.sprite = sp[Random.Range(0, sp.Length)];
        }
        render.transform.localScale = new Vector3(levelScale * ScalePerLevel, levelScale * ScalePerLevel, 1);
    }
    private void Destroy()
    {
        OnDestroy?.Invoke();
        Rest();
    }
    protected override void OnRest()
    {
        OnDead?.Invoke(this);
        base.OnRest();
    }

    public UnityAction OnDestroy;
    public UnityAction<DamageData> OnHit;
    public UnityAction<Meteorite> OnDead;

    public void StartUp(float speed, Vector2 DirectMove, int hp)
    {
        this.speedAdd = speed;
        this.DirectMove = DirectMove.normalized;
        this.hp = hp;
        transform.rotation = MathQ.DirectionToQuaternion(DirectMove);
        levelScale = (int)(hp / HpPerLevel) + 1;
        maxScale = levelScale;
        UpdateScale();
    }
}
