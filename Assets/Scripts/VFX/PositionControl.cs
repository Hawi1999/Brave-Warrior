using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionControl : MonoBehaviour
{
    private Vector3 StartPosition;
    private Vector3 EndPosition;
    private float TimeMove;

    private float scaled;
    private bool started = false;
    public void SetUp(Vector3 startS, Vector3 endS, float time)
    {
        StartPosition = startS;
        EndPosition = endS;
        TimeMove = time;
    }

    private void Update()
    {
        if (!started)
            return;
        float a = Time.deltaTime / TimeMove;
        scaled += a;
        if (scaled > 1)
        {
            Complete();
            return;
        }
        Vector3 positionCurrent = transform.position;
        transform.position = positionCurrent + (EndPosition - StartPosition) * a;
    }

    public void StartAnimation()
    {
        started = true;
        scaled = 0;
    }

    void Complete()
    {
        transform.position = EndPosition;
        Destroy(this);
    }
}
