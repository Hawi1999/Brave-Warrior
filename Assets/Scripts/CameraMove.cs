using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMove : MonoBehaviour
{
    public static CameraMove Instance{
        get; private set;
    }
    private Vector2 currentVelocity;
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float MaxSpeed = 4;
    private PlayerController Player
    {
        get
        {
            return PlayerController.Instance;
        }
    }

    float Xmin, Xmax, Ymin, Ymax;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Player == null)
        {
            Debug.LogWarning("Khong tim thay Player");
            return;
        }
        Vector3 old_position = transform.position;
        Vector3 target_pos = targetMove();
        MoveSmooth(old_position, target_pos);
    }

    void MoveSmooth(Vector3 oldPosition, Vector3 target)
    {
        Vector3 new_pos;
        new_pos.x = Mathf.Clamp(Mathf.SmoothDamp(oldPosition.x, target.x, ref currentVelocity.x, smoothTime, MaxSpeed), Xmin, Xmax);
        new_pos.y = Mathf.Clamp(Mathf.SmoothDamp(oldPosition.y, target.y, ref currentVelocity.y, smoothTime, MaxSpeed), Ymin, Ymax);
        new_pos.z = -10;
        transform.position = new_pos;
    }

    Vector3 targetMove()
    {
        if (!Player.HasEnemyNear)
            return Player.getPosition();
        return (Player.TargetFire.PositionColliderTakeDamage + Player.getPosition())/2;
    }



    public void setLimit(Vector3 min, Vector3 max)
    {
        if (min == max)
        {
            Xmin = - Mathf.Infinity;
            Ymin = - Mathf.Infinity;
            Xmax = Mathf.Infinity;
            Ymax = Mathf.Infinity;
        }

        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        Xmin = min.x + width / 2;
        Ymin = min.y + height / 2;
        Xmax = max.x - width / 2;
        Ymax = max.y - height / 2; 
    }
}
