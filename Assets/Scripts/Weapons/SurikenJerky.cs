using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SurikenJerky : MonoBehaviour
{
    [SerializeField] float _distanceJerky;
    [SerializeField] float timeA = 0.2f;
    [SerializeField] float timeB = 0.4f;
    [SerializeField] Transform move;

    Vector2 m_Position_Kerky => new Vector2(_distanceJerky, 0);

    Suriken target;
    bool uping;
    bool downping;
    float targetTime;
    float currenttime;
    Vector2 targetPos;
    Vector2 startPos;
    bool vfxing = false;
    private void Awake()
    {
        target = GetComponent<Suriken>();
        target.OnAttacked += StartJerky;
    }

    private void Update()
    {
        if (vfxing)
        {
            currenttime += Time.deltaTime;
            if (uping)
            {
                if (currenttime > targetTime)
                {
                    move.localPosition = targetPos;
                    currenttime = 0;
                    targetTime = timeB;
                    uping = false;
                    downping = true;
                    startPos = m_Position_Kerky;
                    targetPos = Vector2.zero;

                } else
                {
                    move.localPosition = startPos + (targetPos - startPos) * currenttime / targetTime;
                }
            } else if (downping)
            {
                if (currenttime > targetTime)
                {
                    move.localPosition = targetPos;
                    currenttime = 0;
                    targetTime = 0;
                    uping = false;
                    downping = false;
                    vfxing = false;
                }
                else
                {
                    move.localPosition = startPos + (targetPos - startPos) * currenttime / targetTime;
                }
            }
        }
    }

    void StartJerky()
    {
        vfxing = true;
        currenttime = 0;
        targetTime = timeA;
        uping = true;
        downping = false;
        startPos = Vector2.zero;
        targetPos = m_Position_Kerky;
    }

    private void OnDestroy()
    {
        if (target != null)
        {
            target.OnAttacked -= StartJerky;
        }
    }
}
