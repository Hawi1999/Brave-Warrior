using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunLazeJerky : MonoBehaviour
{
    public Vector2 Limit = new Vector2(0.1f, 0.1f);
    [Range(2, 20)]
    public float FPS = 6;

    private float DistanceShake => 1 / FPS;
    private bool shaking = false;
    private float time = 0;
    private bool startShake = false;
    private bool endShake = false;
    Vector3 localPositionBegun = new Vector3(0,0,0);

    private void Update()
    {
        if (shaking)
        {
            time += Time.deltaTime;
            if (time > DistanceShake)
            {
                Shake();
                time -= DistanceShake;
            }
        } else
        {
            if (endShake)
            {
                transform.localPosition = localPositionBegun;
                endShake = false;
            }
        }
    }

    private void Shake()
    {
        transform.localPosition = localPositionBegun +
            new Vector3(Random.Range(-Limit.x, Limit.x), Random.Range(-Limit.y, Limit.y), 0);
    }

    public void OnAttacked()
    {
        startShake = !shaking;
        shaking = true;
    }

    public void OnNotAttack()
    {
        endShake = !shaking;
        shaking = false;
    }

    public void SetLocalPostionBegin()
    {
        localPositionBegun = transform.localPosition;
    }

    public void reset()
    {
    shaking = false;
    time = 0;
    startShake = false;
    endShake = false;
    localPositionBegun = new Vector3(0, 0, 0);
    }
}
