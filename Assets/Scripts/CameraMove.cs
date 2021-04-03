using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TaretVector3
{
    public string id;
    public Vector3 position;

    public TaretVector3(string id, Vector3 position)
    {
        this.id = id;
        this.position = position;
    }
    public void SetPosition(Vector3 a)
    {
        position = a;
    }
}

public class CameraMove : MonoBehaviour
{
    public static CameraMove Instance{
        get; private set;
    }
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
    List<TaretVector3> targets = new List<TaretVector3>();

    public void AddPosition(TaretVector3 target)
    {
        for (int i = 0; i < targets.Count;i++)
        {
            if (target.id == targets[i].id)
            {
                targets[i].SetPosition(target.position);
                return;
            }
        }
        targets.Add(target);
    }

    public void RemovePosition(string id)
    {
        for (int i = targets.Count - 1; i > -1; i--)
        {
            if (id == targets[i].id)
            {
                targets.RemoveAt(i);
                return;
            }
        }
    }
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
        Move();
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

    void Move()
    {
        if (targets.Count == 0)
        {
            return;
        }
        Vector3 target = Vector3.zero;
        string s = string.Empty;
        foreach (TaretVector3 t in targets)
        {
            s += t.id + ":" +  t.position + "\n";
            target += t.position;
        }
        target /= targets.Count;
        target.z = -10;
        transform.position = Vector3.SmoothDamp(transform.position, target, ref currentVelocity3, smoothTime);
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
