using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRoom : Door
{
    [SerializeField] SpriteRenderer render;
    [SerializeField] AnimationQ animate;
    [SerializeField] Collider2D col;
    [SerializeField] BuiControl buicontrol;

    private void Start()
    {
        SetUpVariable();
        AfterSetUpVarialbe();
        Open();
    }

    void SetUpVariable()
    {
        if (animate == null) animate = GetComponent<AnimationQ>();
        if (render == null) render = GetComponent<SpriteRenderer>();
        if (col == null) col = GetComponent<Collider2D>();
        if (buicontrol == null) buicontrol = GetComponent<BuiControl>();
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
    public void OnAnimateFinished(string code)
    {
        if (code == "Open")
        {
            if (render != null)
                render.sprite = null;
            if (col != null)
                col.enabled = false;
            if (buicontrol != null)
            {
                buicontrol.SpawnBui(25);
            }
        }
        if (code == "Close")
        {
            if (buicontrol != null)
            {
                buicontrol.SpawnBui(25);
            }
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
}
