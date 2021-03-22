using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomDrop : MonoBehaviour
{
    [SerializeField] private int Damage = 10;
    [SerializeField] private int Radius = 3;
    [SerializeField] float timeDrop = 2f;
    [SerializeField] private float DistanceDrop = 3f;
    [SerializeField] private LayerMask layerTarget;
    [SerializeField] private SpriteRenderer Egg;
    [SerializeField] private SpriteRenderer Zone;
    [SerializeField] private Gradient color;
    [SerializeField] private ControlPartice par;
    DamageData damage;
    public void Awake()
    {
        gameObject.SetActive(false);
        Egg.color = color.Evaluate(0);
        Zone.color = color.Evaluate(0);
        Egg.sortingLayerName = "Effect";
        Zone.sortingLayerName = "Current";
        Zone.sortingOrder = (int)(-10f * transform.position.y);
    }

    public void StartDrop(DamageData damage)
    {
        this.damage = damage;
        Egg.transform.position = transform.position + DistanceDrop * Vector3.up;
        gameObject.SetActive(true);
        iTween.MoveTo(Egg.gameObject, iTween.Hash(
            "position", transform.position,
            "time", timeDrop,
            "easetype", iTween.EaseType.easeInCubic,
            "oncompletetarget", gameObject,
            "oncomplete", "Dropped"));
        iTween.ValueTo(Egg.gameObject, iTween.Hash(
            "from", 0,
            "to", 1,
            "time", timeDrop,
            "onupdate", "UpdateColorRender",
            "easetype", iTween.EaseType.easeInSine));
    }

    private void UpdateColorRender(float a)
    {
        Egg.color = color.Evaluate(a);
        Zone.color = color.Evaluate(a);
    }

    private void Dropped()
    {
        Egg.gameObject.SetActive(false);
        Zone.enabled = false;
        par.Play();
        Explose();
        StartCoroutine(WaitForDestroy());
    }

    public Vector3 GetPositionToDrop(Vector3 position)
    {
        return position + Vector3.up * DistanceDrop;
    }

    public Vector3 GetLocalPositionDrop()
    {
        return Vector3.down * DistanceDrop;
    }

    private void Explose()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, Radius, layerTarget);
        if (cols == null || cols.Length == 0)
        {
            return;
        } else
        {
            foreach(Collider2D col in cols)
            {
                if (col.TryGetComponent(out ITakeHit take))
                {
                    DamageData damage = this.damage.Clone;
                    Vector3 target = take.GetCollider().bounds.center;
                    damage.Damage = (int)(this.Damage * Mathf.Clamp01((Radius - (Vector2.Distance(target, transform.position))) / Radius));
                    damage.BackForce = 5 * Mathf.Clamp01((Radius - (Vector2.Distance(target, transform.position))) / Radius) * Radius;
                    damage.Direction = (target - transform.position).normalized;
                    take.TakeDamaged(damage);
                }
            }
        }
    }

    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }



}
