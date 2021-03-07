using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ControlPartice : PoolingBehaviour
{
    public ParticleSystem par;
    public bool AutoDeactive = true;
    public float timeToDeactive = 1;
    [HideInInspector]
    public bool IsPlaying;
    float time;
    void Awake()
    {
        if (par == null)
        {
            par = GetComponent<ParticleSystem>();
        }
        Stop();
    }

    private void Update()
    {
        if (IsPlaying)
        {
            if (AutoDeactive)
            {
                time += Time.deltaTime;
                if (time >= timeToDeactive)
                {
                    Rest();
                    IsPlaying = false;
                }
            }
        }
    }

    public void SetStartColor(Color c)
    {
        par.startColor = c;
    }

    public void Play()
    {
        gameObject.SetActive(true);
        par.Play();
        IsPlaying = true;
        time = 0;
    }

    public void Stop()
    {
        par.Stop();
        IsPlaying = false;
        Rest();
    }

    public void Emit(int a)
    {
        par.Emit(a);
    }
}
