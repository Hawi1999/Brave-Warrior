using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public enum DirectConnect
{
    Left,
    Right,
    Up
}

public static class DrawMap 
{
    public static Tilemap tilemapCR => TileManager.TileCurrent;
    public static Tilemap tilemapN => TileManager.TileBack;

    public static void Draw(Texture2D text, Vector3Int center)
    {
        TileDatas wall = DataMap.GetWallTiles();
        TileDatas back = DataMap.GetBackTiles();
        if (tilemapCR == null)
        {
            Debug.Log("Null CR");
        }
        if (tilemapN == null)
        {
            Debug.Log("Null N");
        }
        Vector3Int StartPosition = center - new Vector3Int(text.width / 2, text.height / 2, 0) - new Vector3Int(1,1,0);
        for (int x = StartPosition.x; x < StartPosition.x + text.width + 2; x++)
        {
            for (int y = StartPosition.y; y < StartPosition.y + text.height + 2; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (x == StartPosition.x || 
                    x == StartPosition.x + text.width + 1 ||
                    y == StartPosition.y || 
                    y == StartPosition.y + text.height + 1)
                {
                    if (wall != null)
                    {
                        tilemapCR.SetTile(pos, wall.GetTile());
                    } 
                } else
                {
                    Vector2Int pixelVector = (Vector2Int)(pos - StartPosition) - Vector2Int.one;
                    TileDatas tiles = DataMap.GetBarrierTiles(text.GetPixel(pixelVector.x, pixelVector.y));
                    if (tiles != null)
                    {
                        tilemapCR.SetTile(pos, tiles.GetTile());
                    }
                    if (back != null)
                    {
                        tilemapN.SetTile(pos, back.GetTile());
                    }
                }
            }
        }
    }
    // Hãy gọi hàm này sau khi Draw hết tất cả cá Round
    public static void DrawConnect(Vector3Int start, Vector3Int end)
    {
        //Vertical
        if (start.x == end.x)
        {
            // Xóa Tile đễ làm cầu dẫn
            for (int i = start.x - 2; i < start.x + 2; i++)
            {
                Vector3Int pos = new Vector3Int(i, start.y - 1, 0);
                tilemapCR.SetTile(pos, null);
                pos = new Vector3Int(i, end.y, 0);
                tilemapCR.SetTile(pos, null);
            }
            // Xây Cầu dẫn
            TileDatas wall = DataMap.GetWallTiles();
            if (wall == null)
            {
                Debug.Log("Chưa có dữ liệu TileWall cho " + MAP_GamePlay.CodeMapcurent.ToString() + " mà đã chạy rồi, chán thanh niên");
                return;
            }
            for (int i = start.y; i < end.y; i++)
            {
                Vector3Int pos = new Vector3Int(start.x - 3, i, 0);
                Tile tile = wall.GetTile();
                tilemapCR.SetTile(pos, tile);
                pos = new Vector3Int(start.x + 2, i, 0);
                tile = wall.GetTile();
                tilemapCR.SetTile(pos, tile);
            }
            // Xây Back
            TileDatas back = DataMap.GetBackTiles();
            for (int x = start.x - 2; x < start.x + 2; x++)
            {
                for (int y = start.y - 1; y < end.y + 1; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    tilemapN.SetTile(pos, back.GetTile());
                }
            }

        }
        // Horizotal
        else if (start.y == end.y)
        {
            // Xóa Tile đễ làm cầu dẫn
            for (int i = start.y - 2; i < start.y + 2; i++)
            {
                Vector3Int pos = new Vector3Int(start.x - 1, i, 0);
                tilemapCR.SetTile(pos, null);
                pos = new Vector3Int(end.x, i, 0);
                tilemapCR.SetTile(pos, null);
            }
            // Xây Cầu dẫn
            TileDatas wall = DataMap.GetWallTiles();
            if (wall == null)
            {
                Debug.Log("Chưa có dữ liệu TileWall cho " + MAP_GamePlay.CodeMapcurent.ToString() + " mà đã chạy rồi, chán thanh niên");
                return;
            }
            for (int i = start.x; i < end.x; i++)
            {
                Vector3Int pos = new Vector3Int(i, start.y - 3, 0);
                Tile tile = wall.GetTile();
                tilemapCR.SetTile(pos, tile);
                pos = new Vector3Int(i, start.y + 2, 0);
                tile = wall.GetTile();
                tilemapCR.SetTile(pos, tile);
            }
            // Xây Back
            TileDatas back = DataMap.GetBackTiles();
            for (int x = start.x - 1; x < end.x + 1; x++)
            {
                for (int y = start.y - 2; y < start.y + 2; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    tilemapN.SetTile(pos, back.GetTile());
                }
            }
        }
        else
        {
            Debug.Log(start + " & " + end + " thế này rồi làm cầu dẫn kiểu gì?");
        }
    }
}
