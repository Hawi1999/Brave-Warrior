using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TakeHitWithPartice
{
    public ITakeHit take;
    public ControlPartice control;

    public TakeHitWithPartice(ITakeHit take, ControlPartice con)
    {
        this.take = take;
        this.control = con;
    }
}


public class Lazer : MonoBehaviour
{

    public LineRenderer render;
    public Transform laserFirePoint;
    public LayerMask layerTarget;
    public float MaxDistance = 20;
    public float Width = 1;
    public float timeLifeMax = 0.2f;
    public ControlPartice VFXHit;
    public ControlPartice VFXShoot;

    [Header("Preview")]
    public bool update = true;
    float lastMaxDistance;
    bool hiden;
    bool showed;
    private void Awake()
    {
        VFXHit.Stop();
        VFXShoot.Stop();
    }

    public RaycastHit2D GetResult(out bool hasTarget)
    {
        RaycastHit2D[] rays = Physics2D.RaycastAll(laserFirePoint.position, MathQ.QuaternionToDirection(transform.rotation), MaxDistance, layerTarget);
        lastMaxDistance = MaxDistance;
        hasTarget = false;
        RaycastHit2D rayss = new RaycastHit2D();
        if (rays == null || rays.Length == 0)
        {
            
        } else
        {
            foreach (RaycastHit2D ray in rays)
            {
                if (ray.collider.GetComponent<ITakeHit>() != null)
                {
                    lastMaxDistance = Vector2.Distance(ray.point, laserFirePoint.position);
                    hasTarget = true;
                    rayss = ray;
                    break;
                }
            }
        }
        if (!showed)
        {
            ShowVFXHit();
            showed = true;
        }
        UpdateVFXHit();
        return rayss;
    }

    private void Update()
    {
        if (render.enabled)
        {
            DrawLine();
        }
    }

    public void Lit(out RaycastHit2D result, out bool hasTarget)
    {
        render.enabled = true;
        hiden = false;
        result = GetResult(out hasTarget);
        DrawLine();
    }

    public void UnLit()
    {
        showed = false;
        if (!hiden)
        {
            HideVFXHit();
            render.enabled = false;
            hiden = true;
        }
    }

    private void DrawLine()
    {
        render.SetPosition(0, laserFirePoint.localPosition);
        render.SetPosition(1, laserFirePoint.localPosition + Vector3.right * lastMaxDistance);
    }

    private void ShowVFXHit()
    {
        if (!VFXHit.isActiveAndEnabled)
        {
            VFXHit.gameObject.SetActive(true);
        }
        if (!VFXShoot.isActiveAndEnabled)
        {
            VFXShoot.gameObject.SetActive(true);
        }
        VFXHit.transform.rotation = transform.rotation;

    }

    private void UpdateVFXHit()
    {
        ShowVFXHit();
        VFXHit.transform.localPosition = laserFirePoint.localPosition + Vector3.right * lastMaxDistance;
    }

    private void HideVFXHit()
    {
        VFXHit.gameObject.SetActive(false);
        VFXShoot.gameObject.SetActive(false);
    }
    private void OnValidate()
    {
        if (update)
        {
            if (render != null)
            {
                if (laserFirePoint == null)
                    render.SetPosition(0, transform.position);
                else
                    render.SetPosition(0, laserFirePoint.localPosition);
                render.SetPosition(1, Vector3.right * MaxDistance);
                render.startWidth = Width * 3/4;
                render.endWidth = Width;
            }
        }
    }


}
