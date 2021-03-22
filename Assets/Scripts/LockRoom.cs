using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRoom : Door, IBattle
{
    [SerializeField] SpriteRenderer render;
    [SerializeField] AnimationQ animate;
    [SerializeField] Collider2D col;
    [SerializeField] ParticleSystem VFX;

    private void Start()
    {
        SetUpVariable();
        AfterSetUpVarialbe();
    }

    void SetUpVariable()
    {
        if (animate == null) animate = GetComponent<AnimationQ>();
        if (render == null) render = GetComponent<SpriteRenderer>();
        if (col == null) col = GetComponent<Collider2D>();
    }
    void AfterSetUpVarialbe()
    {
        if (animate != null)
        {
            animate.OnAnimateFinished += OnAnimateFinished;
        }
    }

    public override void Open()
    {
        if (animate != null)
        {
            animate.setAnimation("Open");
        }
    }
    public void OnAnimateFinished(Animate animate)
    {
        if (animate.code == "Open")
        {
            if (render != null)
                render.sprite = null;
            if (col != null)
                col.enabled = false;
            if (VFX != null)
                VFX.Play();
        }
        if (animate.code == "Close")
        {
            if (VFX != null)
                VFX.Play();
        }
    }

    public override void Close()
    {
        if (animate != null)
        {
            animate.setAnimation("Close");
        }
        if (col != null)
        {
            col.enabled = true;
        }
    }
    private void Update()
    {
        if (transform.hasChanged && render != null)
        {
            render.sortingOrder = (int)(-10f * transform.position.y);
        }
    }

    public void OnGameStarted()
    {
        Open();
    }

    public void OnGameEnded()
    {
        
    }
}
