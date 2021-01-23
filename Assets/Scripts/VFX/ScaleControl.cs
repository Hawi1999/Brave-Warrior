using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleControl : MonoBehaviour
{
    private Vector3 StartScale;
    private Vector3 EndScale;
    private float TimeScale;

    private float scaled;
    private bool started = false;
    public void SetUp(Vector3 startS, Vector3 endS, float time)
    {
        StartScale = startS;
        EndScale = endS;
        TimeScale = time;
    }

    private void Update()
    {
        if (!started)
            return;
        float a = Time.deltaTime / TimeScale;
        scaled += a;
        if (scaled > 1)
        {
            Complete();
            return;
        }
        Vector3 scaleCurrent = transform.localScale;
        transform.localScale = scaleCurrent + (EndScale - StartScale) * a;    
    }

    public void StartAnimation()
    {
        started = true;
        scaled = 0;
    }

    void Complete()
    {
        transform.localScale = EndScale;
        Debug.Log(EndScale);
        Destroy(this);
    }


}
