using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ControlPartice : PoolingBehaviour
{
    public ParticleSystem par;
    public float timeToDeactive = 1;
    bool playing;
    float time;
    void Awake()
    {
        if (par == null)
        {
            par = GetComponent<ParticleSystem>();
        }
    }

    private void Update()
    {
        if (playing)
        {
            time += Time.deltaTime;
            if (time >= timeToDeactive)
            {
                Rest();
                playing = false;
            }
        }
    }

    public void Play()
    {
        par.Play();
        playing = true;
        time = 0;
    }

    public void Stop()
    {
        par.Stop();
        playing = false;
        Rest();
    }

    public void Emit(int a)
    {
        par.Emit(a);
    }
}
