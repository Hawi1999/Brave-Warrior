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

[System.Serializable]
public class ColorToTiles
{
    public Color color;
    public Tile[] tile;
}
public class DataMap : MonoBehaviour
{
    public static DataMap Instance
    {
        get; private set;
    }
    [SerializeField] public LevelEnemy[] _LevelEnemys;
    [SerializeField] public ColorToTiles[] _TilesData;
    [SerializeField] public ColorToTile[] _TileData;
    [SerializeField] public Texture2D[] _Map1;
    [SerializeField] public LockRoom _Door;
    private void Awake()
    {
        Instance = this;
    }

    public static Texture2D getRandomSpriteMap()
    {
        if (Instance._Map1 == null || Instance._Map1.Length == 0)
        {
            return null;
        }
        return Instance._Map1[UnityEngine.Random.Range(0, Instance._Map1.Length)];
    }

    public static Tile getTileByColor(Color color)
    {
        if (Instance._TilesData != null && Instance._TilesData.Length != 0)
        {
            foreach (ColorToTiles colorToTiles in Instance._TilesData)
            {
                if (colorToTiles.color == color)
                {
                    if (colorToTiles.tile != null && colorToTiles.tile.Length != 0)
                    {
                        return colorToTiles.tile[UnityEngine.Random.Range(0, colorToTiles.tile.Length)];
                    }
                }
            }
        }
        if (Instance._TileData != null && Instance._TileData.Length != 0)
        {
            return Array.Find(Instance._TileData, e => e.color == color)?.tile;
        }
        return null;
    }

    public static List<Enemy> getListEnemyByLevel(int level_total,int minLevel, int maxLevel)
    {
        List<LevelEnemy> listEnable = new List<LevelEnemy>();
        int total_Uutien = 0;
        // Lấy danh sách có thể có Enemy
        foreach (LevelEnemy le in Instance._LevelEnemys)
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
