using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class DataMap : MonoBehaviour
{
    static string Path = "DataMap";

    static EnemyDatas[] _EnemyDatas;
    static TileDatas[] _TileDatas;
    static TextureMapDatas[] _Maps;
    static RoundDatas[] _RoundDatas;
    static LockRoomDatas[] _Doors;
    public static void LoadData()
    {
        _EnemyDatas = Resources.LoadAll<EnemyDatas>(Path);
        _TileDatas = Resources.LoadAll<TileDatas>(Path);
        _Maps = Resources.LoadAll<TextureMapDatas>(Path);
        _Doors = Resources.LoadAll<LockRoomDatas>(Path);
        _RoundDatas = Resources.LoadAll<RoundDatas>(Path);
    }

    public static LockRoom GetLockRoomPrefab(LockRoomDatas.Direct direct)
    {
        return Array.Find(_Doors, e => e.codeMap == MAP_GamePlay.CodeMapcurent && e.direct == direct).Preafabs;
    }
    public static TileDatas GetTileDatas(Color color)
    {
        if (_TileDatas != null && _TileDatas.Length != 0)
        {
            foreach (TileDatas colorToTiles in _TileDatas)
            {
                if (colorToTiles.EqualsColor(color))
                {
                    return (colorToTiles);
                }
            }
        }
        return null;
    }
    public static Texture2D GetSpriteMap(CodeMap c, TypeRound r)
    {
        foreach (TextureMapDatas t in _Maps)
        {
            if (t.EqualCodes(c, r))
            {
                return t.GetTexture2D();
            }
        }
        return null;
    }
    public static List<Enemy> GetListEnemyPrefab(int level_total,int minLevel, int maxLevel)
    {
        List<EnemyDatas> listEnable = new List<EnemyDatas>();
        int total_Uutien = 0;
        // Lấy danh sách có thể có Enemy
        foreach (EnemyDatas le in _EnemyDatas)
        {
            if (le.inGroup(MAP_GamePlay.CodeEnemyCurrent) && le.type == TypeEnemy.Normal && le.Level <= maxLevel && le.Level >= minLevel)
            {
                listEnable.Add(le);
                total_Uutien += le.Priority;
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
                if (listEnable[j].Priority < listEnable[i].Priority)
                {
                    EnemyDatas a = listEnable[i];
                    listEnable[i] = listEnable[j];
                    listEnable[j] = a;
                }
            }
        }
        // Tao danh sach Enemy
        List<Enemy> listEnemy = new List<Enemy>();
        int MaxEnemy = 50;
        do
        {
            int a = UnityEngine.Random.Range(1, total_Uutien + 1);
            EnemyDatas le = GetEnemy(listEnable, a);
            level_total -= le.Level;
            if (level_total < 0)
                break;
            listEnemy.Add(le.GetEnemy);
            MaxEnemy++;
        } while (true || MaxEnemy >= 50);
        return listEnemy;
    }
    public static EnemyDatas GetEnemy(List<EnemyDatas> listEnable, int a)
    {
        int cr = 0;
        for (int i = 0; i < listEnable.Count; i++)
        {
            cr += listEnable[i].Priority;
            if (a <= cr)
            {
                return listEnable[i];
            }
        }
        return new EnemyDatas();
    }
    public static RoundDatas GetRoundDatas()
    {
        return (Array.Find(_RoundDatas, e => e.codeMap == MAP_GamePlay.CodeMapcurent));
    }
    public static TileDatas GetWallTiles()
    {
        CodeMap codeMap = MAP_GamePlay.CodeMapcurent;
        foreach (TileDatas dt in _TileDatas)
        {
            if (dt.codeMap == codeMap && dt.type == TypeTile.Wall)
            {
                return dt;
            }
        }
        return null;
    }
    public static TileDatas GetBarrierTiles(Color color)
    {
        foreach (TileDatas dt in _TileDatas)
        {
            if (dt.codeMap == MAP_GamePlay.CodeMapcurent && dt.EqualsColor(color) && dt.type == TypeTile.Current)
            {
                return dt;
            }
        }
        return null;
    }
    public static TileDatas GetBackTiles()
    {
        CodeMap codeMap = MAP_GamePlay.CodeMapcurent;
        foreach (TileDatas dt in _TileDatas)
        {
            if (dt.codeMap == codeMap && dt.type == TypeTile.Land)
            {
                return dt;
            }
        }
        return null;
    }
    public static EnemyDatas GetEnemyDatasBossPrefab()
    {
        CodeMap codeMap = MAP_GamePlay.CodeMapcurent;
        foreach (EnemyDatas le in _EnemyDatas)
        {
            if (le.codeMap == codeMap && le.type == TypeEnemy.Boss)
            {
                return le;
            }
        }
        return null;
    }
}
