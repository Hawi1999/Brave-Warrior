using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class VFXSpawnDestroyed : VFXSpawn
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] float FPS = 10;
    [SerializeField] bool DestroyOnDone = true;

    SpriteRenderer render;
    float time_current;
    bool complete = false;
    protected override void Start()
    {
        time_current = 0;
        render = GetComponent<SpriteRenderer>();
        render.sortingOrder = (int)(-10f * transform.position.y);
    }

    protected override void Update()
    {
        if (!complete)
        {
            int a = (int)(time_current / (1 / FPS));
            if (a < sprites.Length)
            {
                render.sprite = sprites[a];
            } else
            {
                OnCompleteVFX?.Invoke();
                complete = true;
                if (DestroyOnDone)
                    Destroy(this.gameObject);
            }
            time_current += Time.deltaTime / Time.timeScale;
        } else
        {
            render.sprite = null;

        }


    }
}
