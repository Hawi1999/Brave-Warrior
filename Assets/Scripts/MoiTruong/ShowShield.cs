using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(AutoRotation))]
[RequireComponent(typeof(SpriteRenderer))]
public class ShowShield : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] float a;
    [SerializeField] Vector2 limitSpeed = new Vector2(2, 5);
    AutoRotation rotate;
    SpriteRenderer render;
    Entity target = null;
    float timeout = 0;
    bool outed = false;
    private void Awake()
    {
        rotate = GetComponent<AutoRotation>();
        render = GetComponent<SpriteRenderer>();
    }

    private bool increasing = true;

    private void Update()
    {
        if (outed)
        {
            return;
        }
        if (increasing)
        {
            float s = rotate.speed + Time.deltaTime * a;
            if (s > limitSpeed.y)
            {
                increasing = false;
            }
            rotate.speed = s;
        } else
        {
            float s = rotate.speed - Time.deltaTime * a;
            if (s < limitSpeed.x)
            {
                increasing = true;
            }
            rotate.speed = s;
        }
        
        timeout -= Time.deltaTime;
        if (timeout <= 0)
        {
            OnOutTime?.Invoke();
            EndUp();
            outed = true;
        }
    }

    private void WhenTargetValueChanged(int code)
    {
        if (code == Entity.TRANSFORM)
        {
            UpdatePosition();
        }
        if (code == Entity.SCALESIZE)
        {
            UpdateSize();
        }

    }
    private void UpdatePosition() 
    {
        if (target != null)
        {
            transform.position = target.center;
            render.sortingOrder = target.render.sortingOrder - 1;
        }
    }
    public void UpdateSize()
    {
        if (target != null)
        {
            float ratio = target.ScaleCurrent.Value;
            transform.localScale = new Vector3(ratio, ratio, 1);
        }
    }

    public void SetTarget(Entity host, float time_begin)
    {
        if (target != null)
        {
            target.OnValueChanged -= WhenTargetValueChanged;;
        }
        target = host;
        if (target != null)
        {
            target.OnValueChanged += WhenTargetValueChanged;
        }
        timeout = time_begin;
    }
    void EndUp()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 1f,
            "to", 0,
            "time", 0.5f,
            "onupdate", "endupdate",
            "oncomplete", "endcomplete",
            "easetype", iTween.EaseType.easeInQuart));

    }

    void endupdate(float a)
    {
        float ratio = target.ScaleCurrent.Value * a;
        transform.localScale = new Vector3(ratio, ratio, 1);
    }

    void endcomplete()
    {
        target.OnValueChanged -= WhenTargetValueChanged;
        Destroy(gameObject);
    }

    public void AddTime(float addtime)
    {
        timeout += addtime;
    }

    public UnityAction OnOutTime;

}
