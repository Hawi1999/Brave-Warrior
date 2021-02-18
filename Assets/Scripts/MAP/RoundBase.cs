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
    private GameObject PRSpawnEnemy;
    private List<LockRoom> LockRooms;
    private Vector2Int NuaBanKinh;
    private Vector2Int PositionStartWorld;
    private List<Enemy> ListEnemySpawned;
    private bool started = false;
    private int idmax = 0;
    public BoxCollider2D col => GetComponent<BoxCollider2D>();
    private void Start()
    {
        setMap();
    }

    public virtual Vector2[] getMINMAX()
    {
        Vector2[] vector2 = new Vector2[2];
        vector2[0] = PositionStartWorld;
        vector2[1] = vector2[0] + NuaBanKinh * 2;
        return vector2;
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
        foreach (Enemy LE in listEnemy)
        {

            Vector3 position = getPositonInRound();
            Enemy ene = EnemyManager.Instance.Spawn(LE, position, PRSpawnEnemy.transform, getMINMAX());
            ene.OnDeath += HasEnemyDying;
            ListEnemySpawned.Add(ene);
        }
    }

    private Vector3 getPositonInRound()
    {
        Vector3 Position = transform.position + new Vector3(UnityEngine.Random.Range(-NuaBanKinh.x, NuaBanKinh.x), UnityEngine.Random.Range(-NuaBanKinh.y, NuaBanKinh.y));
       
        int i = 0;
        while (HasCollisionInRound(Position) && i < 100)
        {
            Position = transform.position + new Vector3(UnityEngine.Random.Range(-NuaBanKinh.x, NuaBanKinh.x), UnityEngine.Random.Range(-NuaBanKinh.y, NuaBanKinh.y));
            i++;
        }
        return Position;

    }

    private bool HasCollisionInRound(Vector3 pos)
    {/*
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
        }*/

        /*Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos, new Vector2(0.5f, 0.5f),
            LayerMask.NameToLayer("Wall") | 
            LayerMask.NameToLayer("Barrier") | 
            LayerMask.NameToLayer("River"));
        if (collider2Ds != null && collider2Ds.Length != 0)
        {
            return false;
        }
        return true;*/

        if (tilemap == null)
            return false;
        else
        {
            Vector3Int position = new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), 0);
            return tilemap.GetTile(position) != null;
        }
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
        LockRoom a = Instantiate(DataMap.Instance._Door, transform);
        a.transform.position = col.bounds.center + new Vector3(0, NuaBanKinh.y, 0);
        LockRoom b = Instantiate(DataMap.Instance._Door, transform);
        b.transform.position = col.bounds.center + new Vector3(0, -NuaBanKinh.y - 1, 0);
        LockRooms.Add(a);
        LockRooms.Add(b);
    }


}
