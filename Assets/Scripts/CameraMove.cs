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
    private Vector3 currentVelocity3;
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float MaxSpeed = 4;
    private PlayerController Player
    {
        get
        {
            return PlayerController.PlayerCurrent;
        }
    }

    float Xmin, Xmax, Ymin, Ymax;

    bool isShaking = false;
    float timeShaking = 0;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        PlayerController.OnReceiveDamage += StartShake;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Player == null)
        {
            Debug.LogWarning("Khong tim thay Player");
            return;
        }
        Vector3 target_pos = targetMove();
        MoveSmooth(transform.position, target_pos);
        if (isShaking)
        {
            // Shaking thì sao ?
            Shake();
            timeShaking += Time.deltaTime;
            if (timeShaking > 0.1f)
            {
                isShaking = false;
            }
        }
        FixTransForm();
    }

    void FixTransForm()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, Xmin, Xmax);
        pos.y = Mathf.Clamp(pos.y, Ymin, Ymax);
        pos.z = -10;
        transform.position = pos;
    }
    private void StartShake()
    {
        isShaking = true;
        timeShaking = 0;
    }

    private void Shake()
    {
        transform.position = transform.position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
    }

    void MoveSmooth(Vector3 oldPosition, Vector3 target)
    {
        Vector3 new_pos;
        transform.position = Vector3.SmoothDamp(oldPosition, target, ref currentVelocity3, smoothTime);
        //new_pos.x = Mathf.Clamp(Mathf.SmoothDamp(oldPosition.x, target.x, ref currentVelocity.x, smoothTime, MaxSpeed), Xmin, Xmax);
        //new_pos.y = Mathf.Clamp(Mathf.SmoothDamp(oldPosition.y, target.y, ref currentVelocity.y, smoothTime, MaxSpeed), Ymin, Ymax);
        //new_pos.z = -10;
    }

    Vector3 targetMove()
    {
        if (!Player.HasEnemyAliveNear)
            return Player.center;
        return (Player.TargetFire.center + Player.center)/2;
    }

    private void OnDestroy()
    {
        PlayerController.OnReceiveDamage -= Shake;
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
