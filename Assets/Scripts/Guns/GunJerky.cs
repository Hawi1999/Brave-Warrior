using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GunBase))]
public class GunJerky : MonoBehaviour
{
    [SerializeField] float speedRotationBF = 1080;

    [SerializeField] float speedPositionBF = 5;

    [SerializeField] float TimeBF = 0.05f;
    [SerializeField] float TimeAT = 0.1f;

    float lasttime = 0;
    A statusCurrent = A.Wait;
    public enum A
    {
        Wait,
        BF,
        AT,
    }

    
    GunBase gun => GetComponent<GunBase>();
    Vector3 rotationCurrent
    {
        set
        {
            gun.render.transform.localRotation = Quaternion.Euler(value);
        }
    }

    Vector3 positionCurrent
    {
        set
        {
            gun.render.transform.localPosition = value;
        }
    }

    private void Start()
    {
        gun.OnAttack += OnAttack;
    }
    private void Update()
    {
        switch (statusCurrent)
        {
            case (A.Wait):
                rotationCurrent = Vector3.zero;
                positionCurrent = Vector3.zero;
                break;
            case (A.BF):
                float deltaTime = Time.time - lasttime;
                if (deltaTime < TimeBF)
                {
                    if (gun.isLeftDir)
                        rotationCurrent = new Vector3(0, 0, -speedRotationBF * deltaTime);
                    else
                        rotationCurrent = new Vector3(0, 0, speedRotationBF * deltaTime);
                    positionCurrent = new Vector3(-speedPositionBF * deltaTime, 0);
                } else
                {
                    statusCurrent = A.AT;
                    lasttime = Time.time;
                }
                break;
            case (A.AT):
                deltaTime = Time.time - lasttime;
                if (deltaTime < TimeAT)
                {
                    if (gun.isLeftDir)
                        rotationCurrent = new Vector3(0, 0, -speedRotationBF * (TimeAT - deltaTime)*TimeBF/TimeAT);
                    else
                        rotationCurrent = new Vector3(0, 0, speedRotationBF * (TimeAT - deltaTime) * TimeBF / TimeAT);
                    positionCurrent = new Vector3(-speedPositionBF * (TimeAT -  deltaTime) * TimeBF / TimeAT, 0);
                }
                else
                {
                    statusCurrent = A.Wait;
                }
                break;
        }
    }


    private void OnAttack()
    {
        statusCurrent = A.BF;
        lasttime = Time.time;
    }

    private void OnDestroy()
    {
        gun.OnAttack -= OnAttack;
    }

}
