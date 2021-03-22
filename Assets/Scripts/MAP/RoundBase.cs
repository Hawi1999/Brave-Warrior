using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class RoundBase : MonoBehaviour
{
    BoxCollider2D col;
    bool startedRound = false;
    bool endedRound = false;
    List<LockRoom> LockRooms;
    public static RoundBase RoundCurrent
    {
        get; protected set;
    }
    [HideInInspector]
    public RoundData Data;
    protected virtual void Awake()
    {
        LockRooms = new List<LockRoom>();
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }


    protected virtual void Start()
    {

    }

    protected Tilemap TileMain => TileManager.TileCurrent;
    protected Tilemap TileBack => TileManager.TileBack;
    public virtual void SetUp(RoundData roundData)
    {
        Data = roundData;
        col.size = Data.Size;
        transform.position = (Vector3Int)Data.position;
    }

    public virtual void SetLocKRoom(Direct[] directs)
    {
        LockRooms = new List<LockRoom>();
        for (int i = 0; i < directs.Length; i++)
        {
            LockRoom a = null;
            switch (directs[i])
            {
                case Direct.Left:
                    a = Instantiate(DataMap.GetLockRoomPrefab(LockRoomDatas.Direct.Vertical), transform);
                    a.transform.position = (Vector2)Data.GetPosition(Direct.Left);
                    break;
                case Direct.Right:
                    a = Instantiate(DataMap.GetLockRoomPrefab(LockRoomDatas.Direct.Vertical), transform);
                    a.transform.position = (Vector2)Data.GetPosition(Direct.Right) + Vector2.left;
                    break;
                case Direct.Up:
                    a = Instantiate(DataMap.GetLockRoomPrefab(LockRoomDatas.Direct.Horizontal), transform);
                    a.transform.position = (Vector2)Data.GetPosition(Direct.Up) + Vector2.down;
                    break;
                case Direct.Down:
                    a = Instantiate(DataMap.GetLockRoomPrefab(LockRoomDatas.Direct.Horizontal), transform);
                    a.transform.position = (Vector2)Data.GetPosition(Direct.Down);
                    break;
            }
            if (a != null)
            {
                LockRooms.Add(a);
            }
        }
    }

    protected void OpenAllDoor()
    {
        if (LockRooms != null && LockRooms.Count != 0)
        {
            foreach (LockRoom Lock in LockRooms)
            {
                Lock.Open();
            }
        }
        else
        {
            Debug.Log("Bạn cần phải có của khóa");
        }
    }
    protected void CloseAllDoor()
    {
        if (LockRooms != null && LockRooms.Count != 0)
        {
            foreach (LockRoom Lock in LockRooms)
            {
                Lock.Close();
            }
        }
        else
        {
            Debug.Log("Bạn cần phải có của khóa");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerController.PlayerCurrent.gameObject)
        {
            if (!endedRound)
            {
                OnPLayerLeaveFirst();
                endedRound = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerController.PlayerCurrent.gameObject)
        {
            if (!startedRound)
            {
                OnPLayerOnInFirst();
                startedRound = true;
            }
        }
    }

    protected virtual void OnPLayerOnInFirst()
    {
        RoundCurrent = this;
    }
    protected virtual void OnPLayerLeaveFirst()
    {
        RoundCurrent = null;
    }
}
