using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

[Serializable]
public struct LevelEnemy
{
    public int Level;
    public Enemy Enemy;
    public int UuTien;
}

[System.Serializable]
public class ColorToTile
{
    public Color color;
    public Tile tile;
}
public class DataMap : MonoBehaviour
{
    public static DataMap Instance
    {
        get; private set;
    }
    [SerializeField] private LevelEnemy[] _LevelEnemys;
    public static LevelEnemy[] LevelEnemys;
    [SerializeField] private ColorToTile[] _TileData;
    public static ColorToTile[] TileDatas;
    [SerializeField] private Texture2D[] _MAP20x10;
    public static Texture2D[] MAP20x10;
    [SerializeField] private LockRoom _Door;
    public static LockRoom Door;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
        TileDatas = _TileData;
        MAP20x10 = _MAP20x10;
        LevelEnemys = _LevelEnemys;
        Door = _Door;
    }

    public static Texture2D getRandomSpriteMap()
    {
        if (MAP20x10 == null || MAP20x10.Length == 0)
        {
            return null;
        }
        return MAP20x10[UnityEngine.Random.Range(0, MAP20x10.Length)];
    }

    public static Tile getTileByColor(Color color)
    {
        if (TileDatas == null || TileDatas.Length == 0)
        {
            return null;
        }
        return Array.Find(TileDatas, e => e.color == color)?.tile;
    }

    public static List<Enemy> getListEnemyByLevel(int level_total,int minLevel, int maxLevel)
    {
        List<LevelEnemy> listEnable = new List<LevelEnemy>();
        int total_Uutien = 0;
        // Lấy danh sách có thể có Enemy
        foreach (LevelEnemy le in LevelEnemys)
        {
            if (le.Level <= maxLevel && le.Level >= minLevel)
            {
                listEnable.Add(le);
                total_Uutien += le.UuTien;
            }
        }
        if (listEnable.Count == 0)
        {
            Debug.Log("Data không hợp lệ");
            return null;
        }
        // Sắp xếp
        for (int i = 0; i < listEnable.Count - 1; i++)
        {
            for (int j = i + 1; j < listEnable.Count; j++)
            {
                if (listEnable[j].UuTien < listEnable[i].UuTien)
                {
                    LevelEnemy a = listEnable[i];
                    listEnable[i] = listEnable[j];
                    listEnable[j] = a;
                }
            }
        }
        // Tao danh sach Enemy
        List<Enemy> listEnemy = new List<Enemy>();
        do
        {
            int a = UnityEngine.Random.Range(1, total_Uutien + 1);
            LevelEnemy le = getEnemy(listEnable, a);
            level_total -= le.Level;
            if (level_total < 0)
                break;
            listEnemy.Add(le.Enemy);
        } while (true);
        return listEnemy;
    }

    public static LevelEnemy getEnemy(List<LevelEnemy> listEnable, int a)
    {
        int cr = 0;
        for (int i = 0; i < listEnable.Count; i++)
        {
            cr += listEnable[i].UuTien;
            if (a <= cr)
            {
                return listEnable[i];
            }
        }
        return new LevelEnemy();
    }
}
