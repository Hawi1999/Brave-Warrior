using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

[Serializable]
public class Dot
{
    public int TotalLevel;
    public int MaxLevel;
    public int MinLevel;
}
[RequireComponent(typeof(BoxCollider2D))]
public class RoundBase : MonoBehaviour
{

    public static RoundBase RoundCurrent;
    // cái này dùng để xác định vùng spawn có được able hay không.
    private Tilemap tilemap;
    private List<Dot> Dots;
    protected int Current_IDDot;
    private List<LockRoom> LockRooms;
    [SerializeField] private VFXSpawn VFXSpawnPrefabs;
    private Vector2Int NuaBanKinh;
    private Vector2Int PositionStartWorld;
    private List<Enemy> ListEnemySpawned;
    private GameObject PRSpawnEnemy;
    private bool started = false;
    private int idmax = 0;
    public BoxCollider2D col => GetComponent<BoxCollider2D>();
    private void Start()
    {
        setMap();
    }
    private void StartRound(int a)
    {
        if (a == 0)
        {
            CloseAllDoor();
        }
        SpawnEnemy(a);
        OnStartRound(a);
    }
    private void EndRound(int a)
    {
        OnEndRound(a);
        NextRound(a + 1);
    }
    private void HasEnemyDying(Entity entity)
    {
        Enemy enemy;
        if (entity is Enemy)
        {
            enemy = entity as Enemy;
        } else
        {
            return;
        }
        ListEnemySpawned.Remove(enemy);
        OnHasEnemyDie(enemy);
        if (ListEnemySpawned.Count == 0)
        {
           EndRound(Current_IDDot);
        }
    }
    private void NextRound(int id_new)
    {
        RoundCurrent = this;
        if (id_new >= idmax)
        {
            Clear();
        } else
        {
            Current_IDDot = id_new;
            StartRound(Current_IDDot);
        }
    }
    private void Clear()
    {
        RoundCurrent = null;
        OpenAllDoor();
        OnClear();
    }
    private void OpenAllDoor()
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
    private void CloseAllDoor()
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
    private void SpawnEnemy(int a)
    {
        if (PRSpawnEnemy == null)
        {
            PRSpawnEnemy = Instantiate(new GameObject("PR Enemy Spawned"), transform);
        }
        if (Dots == null)
        {
            Clear();
            return;
        }
        List<Enemy> listEnemy = DataMap.getListEnemyByLevel(Dots[a].TotalLevel, Dots[a].MinLevel, Dots[a].MaxLevel);
        if (listEnemy == null || listEnemy.Count == 0)
        {
            Debug.Log("Không có danh sánh Enemy trong đợt " + a + ", tiếp tục đợt tiếp theo.");
            NextRound(a + 1);
            return;
        }
        if (ListEnemySpawned == null)
        {
            ListEnemySpawned = new List<Enemy>();
        }
        if (VFXSpawnPrefabs == null)
        {
            Debug.Log("Không có VFXSpawn, không thể spawn enemy");
            return;
        }
        foreach (Enemy LE in listEnemy)
        {
            Vector3 position = getPositonInRound();
            VFXSpawn v = Instantiate(VFXSpawnPrefabs, position, Quaternion.identity, PRSpawnEnemy.transform);
            Enemy enemy = Instantiate(LE, position, Quaternion.identity, PRSpawnEnemy.transform);
            enemy.gameObject.SetActive(false);
            ListEnemySpawned.Add(enemy);
            enemy.OnDeath += HasEnemyDying;
            v.OnCompleteVFX += () => HienEnemy(enemy);
        }
    }

    private void HienEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private Vector3 getPositonInRound()
    {
        Vector3 Position = transform.position + new Vector3(UnityEngine.Random.Range(-NuaBanKinh.x, NuaBanKinh.x), UnityEngine.Random.Range(-NuaBanKinh.y, NuaBanKinh.y));
        if (tilemap == null)
        {
            return Position;
        } else
        {
            while (HasCollisionInRound(Position))
            {
                Position = transform.position + new Vector3(UnityEngine.Random.Range(-NuaBanKinh.x, NuaBanKinh.x), UnityEngine.Random.Range(-NuaBanKinh.y, NuaBanKinh.y));
            }
            return Position;
        }

    }

    private bool HasCollisionInRound(Vector3 pos)
    {
        if (tilemap != null)
        {
            Vector3Int newPos = new Vector3Int(Mathf.CeilToInt(pos.x) - 1, Mathf.CeilToInt(pos.y) - 1, 0);
            GameObject ins = tilemap.GetInstantiatedObject(newPos);
            if (ins != null)
            {
                BoxCollider2D col = ins.GetComponent<BoxCollider2D>();
                if (col != null && !col.isTrigger)
                {
                    if (col.bounds.Contains(pos))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    protected virtual void OnStartRound(int id)
    {

    }

    protected virtual void OnClear()
    {
        if (ChestManager.Instance)
        {
            ChestManager.Instance.ReWardChest(TypeChest.Copper, transform.position);
        }
    }

    protected virtual void OnEndRound(int a)
    {

    }

    protected virtual void OnHasEnemyDie(Enemy enemy)
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (started)
                return;
            else
            {
                started = true;
                NextRound(0);
            }
        }
    }
    public void setUp(Vector3Int position, List<Dot> dots, Tilemap tile, Vector2Int r)
    {
        transform.position = position;
        Dots = dots;
        tilemap = tile;
        NuaBanKinh = r;
        PositionStartWorld = (Vector2Int)position - NuaBanKinh;
    }

    private void setMap()
    {
        col.size = 2 * NuaBanKinh;
        if (Dots != null)
        {
            idmax = Dots.Count;
        }
        Texture2D image = DataMap.getRandomSpriteMap();
        if (image == null)
        {
            return;
        }
        for (int i = 0; i < image.width; i++)
        {
            for (int j = 0; j < image.height; j++)
            {
                Color color = image.GetPixel(i, j);
                if (color == null)
                    continue;
                Tile tile = DataMap.getTileByColor(color);
                if (tile == null)
                {
                    continue;
                }
                tilemap.SetTile((Vector3Int)(PositionStartWorld + new Vector2Int(i, j)), tile);
            }
        }
        LockRooms = new List<LockRoom>();
        LockRoom a = Instantiate(DataMap.Door, transform);
        a.transform.position = col.bounds.center + new Vector3(0, NuaBanKinh.y, 0);
        LockRoom b = Instantiate(DataMap.Door, transform);
        b.transform.position = col.bounds.center + new Vector3(0, -NuaBanKinh.y - 1, 0);
        LockRooms.Add(a);
        LockRooms.Add(b);
    }


}
