using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileManager : MonoBehaviour
{
    public Tilemap tileCurrent;
    public Tilemap tileBack;
    void Awake()
    {
        TileCurrent = tileCurrent;
        TileBack = tileBack;
    }


    public static Tilemap TileCurrent
    {
        get; private set;
    }
    public static Tilemap TileBack
    {
        get; private set;
    }
    public static Vector2 GetPositionInLimit(Vector2 MIN, Vector2 MAX)
    {
        if (TileCurrent == null)
        {
            Debug.Log("Tile Current không tồn tại, trả về giá trị mặc định");
            return (MIN + MAX) / 2;
        }
        return GetPositionInLimit(TileCurrent.WorldToCell(MIN), TileCurrent.WorldToCell(MAX), new Vector2(), false);
    }

    public static Vector2 GetPositionInLimit(Vector2 MIN, Vector2 MAX, Vector2 from, bool CanMoTo = true)
    {
        if (TileCurrent == null)
        {
            Debug.Log("Tile Current không tồn tại, trả về giá trị mặc định");
            return from;
        }
        return GetPositionInLimit(TileCurrent.WorldToCell(MIN), TileCurrent.WorldToCell(MAX), from, CanMoTo);
    }

    public static Vector2 GetPositionInLimit(Vector3Int MIN, Vector3Int MAX, Vector2 center, bool CanMoTove = true)
    {
        List<Vector3Int> list = new List<Vector3Int>();
        for (int i = MIN.y; i <= MAX.y; i++)
        {
            for (int j = MIN.x; j <= MAX.x; j++)
            {
                Vector3Int position = new Vector3Int(j, i, 0);
                if (TileCurrent.GetTile(position) == null)
                {
                    if (CanMoTove)
                    {
                        if (CanMoveTo(center, new Vector2(position.x + 0.5f, position.y + 0.5f))) ;
                        {
                            list.Add(position);
                        }
                    }
                    else
                    {
                        list.Add(position);
                    }
                }
            }
        }
        if (list.Count == 0)
        {
            return center;
        }
        int random = Random.Range(0, list.Count);
        Vector3Int a = list[random];
        return (Vector2)((Vector3)a) + new Vector2(0.5f, 0.5f);
    }

    public static Vector2 GetPositionSquare(Vector2 center, int halfsize)
    {
        Tilemap tile = TileCurrent;
        Vector3Int centerfix = tile.WorldToCell(center);
        Vector3Int MIN = centerfix - Vector3Int.one * halfsize;
        Vector3Int MAX = centerfix + Vector3Int.one * (halfsize - 1);
        return GetPositionInLimit(MIN, MAX, new Vector2(), false);
    }

    public static Vector2 GetPositionSquare(Vector2 center, int halfsize, bool CanMoTo = true)
    {
        Tilemap tile = TileCurrent;
        Vector3Int centerfix = tile.WorldToCell(center);
        Vector3Int MIN = centerfix - Vector3Int.one * halfsize;
        Vector3Int MAX = centerfix + Vector3Int.one * (halfsize - 1);
        return GetPositionInLimit(MIN, MAX, center, CanMoTo);
    }

    public static Vector2 GetPositionInGoundCurrent(Vector2 from, bool CanMoTo = true)
    {
        if (RoundBase.RoundCurrent == null)
        {
            Debug.Log("RoundCurrent Is Null");
            return from;
        }
        if (TileCurrent == null)
        {
            Debug.Log("Tile Current không tồn tại, trả về giá trị mặc định");
            return from;
        }

        RoundBase.RoundCurrent.Data.GetCellLimit(out Vector2Int MIN, out Vector2Int MAX);
        return GetPositionInLimit(MIN, MAX, from, CanMoTo);
    }

    public static Vector2 GetPositionInGoundCurrent()
    {
        if (RoundBase.RoundCurrent == null)
        {
            Debug.Log("RoundCurrent Is Null");
            return new Vector2();
        }
        if (TileCurrent == null)
        {
            Debug.Log("Tile Current không tồn tại, trả về giá trị mặc định");
            return new Vector2();
        }
        RoundBase.RoundCurrent.Data.GetCellLimit(out Vector2Int MIN, out Vector2Int MAX);
        return GetPositionInLimit(MIN, MAX, new Vector2(), false);
    }

    public static Vector2 GetPositionInGoundCurrent(Vector2Int halfsizeNear, Vector2 center, bool CanMoTo = true)
    {
        if (RoundBase.RoundCurrent == null)
        {
            Debug.Log("RoundCurrent Is Null");
            return center;
        }
        if (TileCurrent == null)
        {
            Debug.Log("Tile Current không tồn tại, trả về giá trị mặc định");
            return center;
        }
        Vector3Int centerCell = TileCurrent.WorldToCell(center);
        Vector3Int Min = centerCell - new Vector3Int(halfsizeNear.x - 1, halfsizeNear.y - 1, 0);
        Vector3Int Max = centerCell + new Vector3Int(halfsizeNear.x, halfsizeNear.y, 0);
        RoundEnemy.RoundCurrent.Data.GetCellLimit(out Vector2Int MIN, out Vector2Int MAX);
        MIN.x = Mathf.Max(MIN.x, Min.x);
        MIN.y = Mathf.Max(MIN.y, Min.y);
        MAX.x = Mathf.Min(Max.x, MAX.x);
        MAX.y = Mathf.Min(Max.y, MAX.y);
        Debug.Log(MIN + "&" + MAX);
        return GetPositionInLimit(MIN, MAX, center, CanMoTo);
    }

    private static bool CanMoveTo(Vector2 position, Vector2 to)
    {
        Vector2 dir = (to - position).normalized;
        float dis = Vector2.Distance(position, to);
        RaycastHit2D[] rays = Physics2D.RaycastAll(position, dir, dis);
        foreach (RaycastHit2D ray in rays)
        {
            if (ray.collider.tag.Contains("Tile"))
            {
                return false;
            }
        }
        return true;
    }
}