using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class MAPController : MonoBehaviour
{
    public static MAPController Instance
    {
        get; set;
    }
    [SerializeField]
    private Vector3 MAX;
    [SerializeField]
    private Vector3 MIN;
    public string SceneCurrent;

    public Vector3[] LimitMoveMap;
    protected Transform PRLimitMoveMap;
    [SerializeField]
    private Transform PosDefault;
    protected Transform PRTeleporter;
    [SerializeField]
    private TeleportScene TeleporterPrefabs;
    [SerializeField]
    protected Teleporttion[] LTeleportion;

    protected Vector2Int PositionPlayerStart
    {
        get
        {
            Vector3 p = PosDefault.position;
            return new Vector2Int(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y));
        }
    }
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
        setTeleporter();
    }
    protected virtual void Start()
    {
        CreatePlayer();
        setLimitForCamera();
        setLimitForPlayerMove();
        SetPlayerPositionInMap();
        if (PlayerController.PlayerCurrent != null)
            CameraMove.Instance.transform.position = PlayerController.PlayerCurrent.GetPosition();
    }

    protected virtual PlayerController CreatePlayer()
    {
        if (PlayerManager.Instance == null || PlayerManager.Instance.PlayerPrefabs == null || PlayerManager.Instance.PlayerPrefabs.Count == 0)
        {
            Debug.LogError("Chưa có danh sách nhân vật trong Data");
            return null;
        }
        int id = PlayerManager.IDPlayer;
        PlayerController player;
        if (PlayerManager.Instance.PlayerPrefabs[id] != null)
        {
            player = Instantiate(PlayerManager.Instance.PlayerPrefabs[id]);
        } else
        {
            player = Instantiate(PlayerManager.Instance.PlayerPrefabs[0]);
        }
        PlayerController.PlayerCurrent = player;
        return player;
    }

    void setLimitForCamera()
    {
        Camera cam = Camera.main;
        cam.GetComponent<CameraMove>()?.setLimit(MIN, MAX);
    }
    protected virtual void setTeleporter()
    {
        if (PRTeleporter == null)
        {
            if (LTeleportion == null || LTeleportion.Length == 0)
                return;
            PRTeleporter = Instantiate(new GameObject("Parent Teleporter"), gameObject.transform).transform;
        }
        foreach (Teleporttion tele in LTeleportion)
        {
            if (tele.isVisible == false)
                continue;
            TeleportScene teleportScene = Instantiate(TeleporterPrefabs, PRTeleporter.transform);
            teleportScene.SetUp(tele);
        }
    }
    protected virtual void setLimitForPlayerMove()
    {
        if (PRLimitMoveMap == null)
        {
            PRLimitMoveMap = Instantiate(new GameObject("Parent Litmit Move"), gameObject.transform).transform;
        }
        
        if (LimitMoveMap == null || LimitMoveMap.Length < 3)
        {
            Vector2[] Sizes = new Vector2[4];
            Vector2[] Tam = new Vector2[4];

            Tam[0] = new Vector2(MIN.x, (MAX.y + MIN.y) / 2);
            Tam[1] = new Vector2((MAX.x + MIN.x) / 2, MAX.y);
            Tam[2] = new Vector2(MAX.x, (MAX.y + MIN.y) / 2);
            Tam[3] = new Vector2((MAX.x + MIN.x) / 2, MIN.y);

            Sizes[0] = new Vector2(1, MAX.y - MIN.y);
            Sizes[1] = new Vector2(MAX.x - MIN.x, 1);
            Sizes[2] = new Vector2(1, MAX.y - MIN.y);
            Sizes[3] = new Vector2(MAX.x - MIN.x, 1);

            for (int i = 0; i < 4; i++)
            {
                BoxCollider2D col = Instantiate(new GameObject(), gameObject.transform).AddComponent<BoxCollider2D>();
                col.transform.position = Tam[i];
                col.size = Sizes[i];
            }
        }
        else
        {
            for (int i = 0; i < PRLimitMoveMap.childCount; i++)
            {
                Destroy(PRLimitMoveMap.GetChild(i).gameObject);
            }
            for (int i = 0; i < LimitMoveMap.Length; i++)
            {
                Vector2 pos1 = LimitMoveMap[i % LimitMoveMap.Length];
                Vector2 pos2 = LimitMoveMap[(i + 1) % LimitMoveMap.Length];
                Vector2 position = (pos1 + pos2)/2;
                Vector2 size = new Vector2(Vector2.Distance(pos1, pos2), 1f);
                Vector3 rotation = RotateCollierMap(pos1, pos2);
                GameObject ob = Instantiate(new GameObject(), PRLimitMoveMap.transform);
                BoxCollider2D col = ob.AddComponent<BoxCollider2D>();
                col.isTrigger = false;
                col.size = new Vector2(1, 1);
                ob.transform.position = position;
                ob.transform.rotation = Quaternion.Euler(rotation);
                ob.transform.localScale = new Vector3(size.x, size.y, 1);
            }
        }
    }
    public virtual void SetPlayerPosDefault()
    {
        GameObject player;
        if (PlayerController.PlayerCurrent != null)
            player = PlayerController.PlayerCurrent.gameObject;
        else
        {
            Debug.Log("Khong tim thay Player");
            return;
        }
        player.transform.position = PosDefault.position;
    }
    public virtual void SetPlayerPositionInMap()
    {
        GameObject player;
        if (PlayerController.PlayerCurrent != null)
            player = PlayerController.PlayerCurrent.gameObject;
        else
        {
            Debug.Log("Khong tim thay Player");
            return;
        }
        Teleporttion teleportion = Array.Find(LTeleportion, e => e.ConnectScene == GameController.LastScene);
        if (teleportion == null)
        {
            Debug.Log("Thiết lập vị trí mặc định cho Player");
            SetPlayerPosDefault();
        }
        else
        {
            player.transform.position = teleportion.PositionBack;
        }
    }
    private Vector3 RotateCollierMap(Vector2 start, Vector2 end)
    {
        int k;
        if (end.x < start.x)
        {
            Vector2 temp = start;
            start = end;
            end = temp;
        }
        Vector2 DT = end - start;
        float heso = (end.y - start.y) / (end.x - start.x);
        if (heso > 0)
            k = 1;
        else if (heso < 0)
            k = -1;
        else k = 0;
        return new Vector3(0, 0, Vector2.Angle(DT, new Vector2(1, 0))*k);
    }

    public virtual void LoadScene(string scene)
    {
        if (scene == "TrangTrai")
        {
            Destroy(PlayerController.PlayerCurrent.gameObject);
        }
        GameController.Instance.LoadScene(scene);
    }

    /// <summary>
    /// Hàm này được gọi khi khi qua màn OpenMap
    /// </summary>
    public static UnityEvent OnGameReadyStart;
}
