using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Illusion : MonoBehaviour
{
    public Instance instance;
    float speed_changed_alpha = 2f;
    float amount_current;
    float time_delay;
    float time_delay_current;
    float max_amount;
    SpriteRenderer render;
    public enum Instance
    {
        Control,
        Instance,
    }

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (instance == Instance.Control)
        {
            time_delay_current += Time.deltaTime;
            if (time_delay_current >= time_delay)
            {
                amount_current++;
                CreateIllusion();
                time_delay_current -= time_delay;
                if (amount_current == max_amount)
                {
                    Destroy(this);
                }
            }
        }
        if (instance == Instance.Instance)
        {
            UpdateAlphaRende();
        }
    }

    #region Illusion Control
    public static void Create(SpriteRenderer render, float time, int amount)
    {
        if(render.gameObject.TryGetComponent(out Illusion ill))
        {
            ill.Reset(time, amount);
        } else
        {
            render.gameObject.AddComponent<Illusion>().Reset(time, amount);
        }
    }

    public void Reset(float time, int amount)
    {
        max_amount = amount;
        time_delay = time / amount;
        time_delay_current = 0;
        instance = Instance.Control;
        amount_current = 1;
        CreateIllusion();
    }

    public SpriteRenderer Render => render;

    private void CreateIllusion()
    {
        if (this.render == null)
        {
            Debug.Log("SpriteRenderer's not found to Illusion");
            return;
        }
        Illusion ill = new GameObject("Illusion").AddComponent<Illusion>();
        ill.transform.position = transform.position;
        ill.transform.rotation = transform.rotation;
        SpriteRenderer render = ill.Render;
        render.sortingLayerName = this.render.sortingLayerName;
        render.sortingOrder = this.render.sortingOrder - 10;
        render.sprite = this.render.sprite;
        render.flipX = this.render.flipX;
        render.flipY = this.render.flipY;
        ill.instance = Instance.Instance;
    }

    #endregion

    #region

    private void UpdateAlphaRende()
    {
        Color a = render.color;
        a.a = a.a - speed_changed_alpha * Time.deltaTime;
        if (a.a <= 0)
        {
            Destroy(this.gameObject);
        } else
        {
            render.color = a;
        }
    }

    #endregion
}
