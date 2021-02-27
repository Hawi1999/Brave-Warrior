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
            return PlayerController.PlayerCurrent;
        }
    }

    float Xmin, Xmax, Ymin, Ymax;

    bool isShaking = false;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        PlayerController.OnReceiveDamage += Shake;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Player == null)
        {
            Debug.LogWarning("Khong tim thay Player");
            return;
        }
        isShaking = false;
        CheckStatus();
        Vector3 target_pos = targetMove();
        if (isShaking)
        {
            // Shaking thì sao ?
            iTween.MoveUpdate(gameObject, target_pos, 2.2f);
        }
        else
        {
            MoveSmooth(transform.position, target_pos);
        }
        FixTransForm();
    }

    void CheckStatus()
    {
        iTween[] iTweens = GetComponents<iTween>();
        foreach (iTween iTween in iTweens)
        {
            if (iTween != null)
            {
                if (iTween.type == "shake" && iTween.isRunning)
                {
                    isShaking = true;
                    break;
                }
            }
        }
    }

    void FixTransForm()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, Xmin, Xmax);
        pos.y = Mathf.Clamp(pos.y, Ymin, Ymax);
        pos.z = -10;
        transform.position = pos;
    }
    private void Shake(Vector3 delta, float time)
    {
        iTween.ShakePosition(gameObject, delta, time);
    }

    private void Shake()
    {
        Shake(new Vector3(0.1f, 0.1f), 0.2f);
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
        if (!Player.HasEnemyAliveNear)
            return Player.getPosition();
        return (Player.TargetFire.PositionColliderTakeDamage + Player.getPosition())/2;
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
