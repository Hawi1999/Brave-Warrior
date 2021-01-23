using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AnimationP {
    public string code;
    public Sprite[] sprites;
    public float FPS = 5;
    public bool Loop = true;
}

[RequireComponent(typeof(SpriteRenderer))]
public class AnimationQ : MonoBehaviour
{
    [SerializeField] AnimationP[] Animate;

    string CodeCurrent;
    AnimationP aniCurrent;
    SpriteRenderer render;
    int id;
    int Id
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
            OnIdSet();
        }
    }
    float tim = 0;
    // Start is called before the first frame update
    void Awake()
    {
        render = GetComponent < SpriteRenderer > ();
        aniCurrent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (aniCurrent != null)
        {
            if (tim >= 1/aniCurrent.FPS)
            {
                if (Id >= aniCurrent.sprites.Length - 1)
                {
                    if (aniCurrent.Loop)
                    {
                        Id = 0;
                    } 
                } else
                {
                    Id++;
                }
                tim = 0;
            } else
            {
                tim += Time.deltaTime;
            }
        }
    }

    private void OnIdSet()
    {
        if (aniCurrent.sprites[id] != null)
        {
            render.sprite = aniCurrent.sprites[Id];
            OnRenderChanged?.Invoke(aniCurrent, Id);
            if (id == aniCurrent.sprites.Length - 1)
                OnAnimateFinished?.Invoke(aniCurrent.code);
        }
    }

    public void setAnimation(string code)
    {
        if (aniCurrent != null && aniCurrent.code == code)
            return;
        AnimationP animateP = Array.Find(Animate, e => e.code == code);
        if (animateP == null) return;
        aniCurrent = animateP;
        Id = 0;
        tim = 0;
    }

    public UnityAction<AnimationP,int> OnRenderChanged;
    public UnityAction<string> OnAnimateFinished;
}
