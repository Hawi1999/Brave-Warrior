using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class PoisonZone : TileTakeHit
{
    public float _Time = 2;
    public SpriteRenderer renderZone;
    public LayerMask Target;
    public ParticleSystem vfx;
    private SpriteRenderer render => GetComponent<SpriteRenderer>();
    private BoxCollider2D box => GetComponent<BoxCollider2D>();

    private float lastDamage;
    private float distanceTime = 1f;
    private float timePoi;
    private bool p;
    private bool poisoning = false;
    private void Awake()
    {
        if (renderZone != null)
        {
            renderZone.gameObject.SetActive(false);
            renderZone.sortingLayerName = "LaneMAP";
        }
        vfx?.Stop();
    }
    public override void TakeDamaged(DamageData data)
    {
        if (!poisoning)
        {
            renderZone.gameObject.SetActive(true);
            box.enabled = false;
            render.enabled = false;
            UpdateAlpha(0);
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", 0f, 
                "to", 1f, 
                "time", 1f, 
                "onupdate", "UpdateAlpha",
                "onupdateparams", "a",
                "oncomplete", "CompleteAlpha"));
            vfx?.Play();
            poisoning = true;
            lastDamage = Time.time;
        }
    }

    private void Update()
    {
        if (poisoning)
        {
            if (Time.time - lastDamage > distanceTime)
            {
                Damaging();
                lastDamage = Time.time;
            }
            if (timePoi > _Time && !p)
            {
                iTween.ValueTo(gameObject, iTween.Hash(
                "from", 1f,
                "to", 0f,
                "time", 1f,
                "onupdate", "UpdateAlpha",
                "onupdateparams", "a",
                "oncomplete", "RemoveTile"));
                p = true;
            }
            timePoi += Time.deltaTime;
        }
    }

    void Damaging()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 2f, Target);
        if (collider2Ds == null || collider2Ds.Length == 0)
        {
            return;
        }
        DamageData damageData = new DamageData();
        damageData.Damage = 3;
        damageData.PoisonFrom = true;
        damageData.Type = DamageElement.Poison;
        foreach (Collider2D collider2D in collider2Ds)
        {
            DamageData damage = damageData.Clone;
            ITakeHit take = collider2D.gameObject.GetComponent<ITakeHit>();
            if (take != null && Vector2.Distance(transform.position, take.GetCollider().bounds.center) < 2f)
            {
                damage.BackForce = 0;
                damage.Direction = Vector3.zero;
                take.TakeDamaged(damage);
            }
        }
    }

    void UpdateAlpha(float a)
    {
        Color b = renderZone.color;
        b.a = a;
        renderZone.color = b;
    }




}
