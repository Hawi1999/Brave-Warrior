using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VFXSpawnEnemy : VFXSpawn
{
    SpriteRenderer render;

    protected override void Start()
    {
        base.Start();
        render = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();
        if(transform.hasChanged)
        {
            setLayerSort();
        }
    }
    private void setLayerSort()
    {
        render.sortingOrder = (int)(transform.position.y * -10f);
    }
}
