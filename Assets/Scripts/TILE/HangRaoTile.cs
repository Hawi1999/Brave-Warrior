using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "New Fence", menuName = "Tiles/Fence 2468")]
public class HangRaoTile : Tile
{
    [SerializeField]
    private Sprite[] Sprites;
    [SerializeField]
    private bool VaCham;
    public int Pivot_Offset = 0;
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (go != null)
        {
            SpriteRenderer render = go.GetComponent<SpriteRenderer>();
            render.sprite = GetSpiteTile(position, tilemap);
            render.sortingLayerName = "Current";
            render.sortingOrder = (int)(- go.transform.position.y * 10f) - Pivot_Offset;
        }
        return base.StartUp(position, tilemap, go);
    }
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        Vector2Int[] posr = new Vector2Int[4];
        posr[0] = new Vector2Int(0, -1);
        posr[1] = new Vector2Int(-1, 0);
        posr[2] = new Vector2Int(1, 0);
        posr[3] = new Vector2Int(0, 1);
        tilemap.RefreshTile(position);
        for (int y = 0; y <= 3; y++)
        {
                Vector3Int pos = new Vector3Int(position.x + posr[y].x, position.y + posr[y].y, position.z);
                if (haveIt(tilemap, pos))
                {
                    tilemap.RefreshTile(pos);
                }
        }
    }

    private Sprite GetSpiteTile(Vector3Int position, ITilemap tilemap)
    {
        string com = string.Empty;
        Vector2Int[] posr = new Vector2Int[4];
        posr[0] = new Vector2Int(0, -1);
        posr[1] = new Vector2Int(-1, 0);
        posr[2] = new Vector2Int(1, 0);
        posr[3] = new Vector2Int(0, 1);
        for (int i = 0; i <= 3; i++)
        {
            Vector3Int pos = new Vector3Int(position.x + posr[i].x, position.y + posr[i].y, position.z);
            if (haveIt(tilemap, pos))
            {
                com += "Y";
            }
            else
            {
                com += "N";
            }
        }
        if (com[0] == 'N' && com[1] == 'N' && com[2] == 'N' && com[3] == 'N')
        {
            return Sprites[0];
        }
        if (com[0] == 'Y' && com[1] == 'N' && com[2] == 'N' && com[3] == 'N')
        {
            return  Sprites[1];
        }
        if (com[0] == 'N' && com[1] == 'Y' && com[2] == 'N' && com[3] == 'N')
        {
            return Sprites[2];
        }
        if (com[0] == 'N' && com[1] == 'N' && com[2] == 'Y' && com[3] == 'N')
        {
            return Sprites[3];
        }
        if (com[0] == 'N' && com[1] == 'N' && com[2] == 'N' && com[3] == 'Y')
        {
            return Sprites[4];
        }
        if (com[0] == 'Y' && com[1] == 'Y' && com[2] == 'N' && com[3] == 'N')
        {
            return Sprites[5];
        }
        if (com[0] == 'Y' && com[1] == 'N' && com[2] == 'Y' && com[3] == 'N')
        {
            return Sprites[6];
        }
        if (com[0] == 'N' && com[1] == 'Y' && com[2] == 'N' && com[3] == 'Y')
        {
            return Sprites[7];
        }
        if (com[0] == 'N' && com[1] == 'N' && com[2] == 'Y' && com[3] == 'Y')
        {
            return Sprites[8];
        }
        if (com[0] == 'Y' && com[1] == 'N' && com[2] == 'N' && com[3] == 'Y')
        {
            return Sprites[9];
        }
        if (com[0] == 'N' && com[1] == 'Y' && com[2] == 'Y' && com[3] == 'N')
        {
            return Sprites[10];
        }
        if (com[0] == 'N' && com[1] == 'Y' && com[2] == 'Y' && com[3] == 'Y')
        {
            return Sprites[11];
        }
        if (com[0] == 'Y' && com[1] == 'N' && com[2] == 'Y' && com[3] == 'Y')
        {
            return Sprites[12];
        }
        if (com[0] == 'Y' && com[1] == 'Y' && com[2] == 'N' && com[3] == 'Y')
        {
            return Sprites[13];
        }
        if (com[0] == 'Y' && com[1] == 'Y' && com[2] == 'Y' && com[3] == 'N')
        {
            return Sprites[14];
        }
        if (com[0] == 'Y' && com[1] == 'Y' && com[2] == 'Y' && com[3] == 'Y')
        {
            return Sprites[15];
        }
        return sprite;
    
}
    bool haveIt(ITilemap tilemap, Vector3Int pos)
    {
        return (tilemap.GetTile(pos) == this);
    }
}
