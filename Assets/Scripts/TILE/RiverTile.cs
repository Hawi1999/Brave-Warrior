using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New River", menuName = "Tiles/River")]
public class RiverTile : Tile
{
    [SerializeField]
    private Sprite[] Sprites;
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        return base.StartUp(position, tilemap, go);
    }
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                Vector3Int pos = new Vector3Int(position.x + x, position.y + y, position.z);
                if (haveIt(tilemap, pos))
                {
                    tilemap.RefreshTile(pos);
                }
            }
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        string com = string.Empty;
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (x == 0 && y == 0)
                    continue;
                Vector3Int pos = new Vector3Int(position.x + x, position.y + y, position.z);
                if (haveIt(tilemap, pos))
                {
                    com += "Y";
                }
                else
                {
                    com += "N";
                }
            }
        }
        if (com[1] == 'N' && com[3] == 'N' && com[4] == 'N' && com[6] == 'N')
        {
            tileData.sprite = Sprites[0];
        }
        if (com[1] == 'Y' && com[3] == 'N' && com[4] == 'N' && com[6] == 'N')
        {
            tileData.sprite = Sprites[1];
        }
        if (com[1] == 'N' && com[3] == 'Y' && com[4] == 'N' && com[6] == 'N')
        {
            int a = Random.Range(0, 2);
            tileData.sprite = Sprites[4 + a];
        }
        if (com[1] == 'N' && com[3] == 'N' && com[4] == 'Y' && com[6] == 'N')
        {
            int a = Random.Range(0, 1);
            tileData.sprite = Sprites[2 + a];
        }
        if (com[1] == 'N' && com[3] == 'N' && com[4] == 'N' && com[6] == 'Y')
        {
            tileData.sprite = Sprites[6];
        }
        if (com[1] == 'Y' && com[3] == 'Y' && com[4] == 'N' && com[6] == 'N')
        {
            if (com[0] == 'Y')
            {
                tileData.sprite = Sprites[7];
            }
            else
            {
                int a = Random.Range(0, 2);
                tileData.sprite = Sprites[12 + a];
            }
        }
        if (com[1] == 'Y' && com[3] == 'N' && com[4] == 'Y' && com[6] == 'N')
        {
            if (com[2] == 'Y')
            {
                int a = Random.Range(0, 2);
                tileData.sprite = Sprites[8 + a];
            }
            else
            {
                tileData.sprite = Sprites[14];
            }
        }
        if (com[1] == 'N' && com[3] == 'N' && com[4] == 'Y' && com[6] == 'Y')
        {
            if (com[7] == 'Y')
            {
                tileData.sprite = Sprites[11];
            }
            else
            {
                tileData.sprite = Sprites[17];
            }
        }
        if (com[1] == 'N' && com[3] == 'Y' && com[4] == 'N' && com[6] == 'Y')
        {
            if (com[5] == 'Y')
            {
                tileData.sprite = Sprites[10];
            }
            else
            {
                int a = Random.Range(0, 2);
                tileData.sprite = Sprites[15 + a];
            }
        }
        if (com[1] == 'Y' && com[3] == 'N' && com[4] == 'N' && com[6] == 'Y')
        {
            tileData.sprite = Sprites[65];
        }
        if (com[1] == 'N' && com[3] == 'Y' && com[4] == 'Y' && com[6] == 'N')
        {
            tileData.sprite = Sprites[66];
        }
        if (com[1] == 'Y' && com[3] == 'Y' && com[4] == 'Y' && com[6] == 'N')
        {
            if (com[0] == 'N' && com[2] == 'N')
            {
                int a = Random.Range(0, 2);
                tileData.sprite = Sprites[18 + a];
            }
            if (com[0] == 'N' && com[2] == 'Y')
            {
                int a = Random.Range(0, 2);
                tileData.sprite = Sprites[23 + a];
            }
            if (com[0] == 'Y' && com[2] == 'N')
            {
                tileData.sprite = Sprites[29];
            }
            if (com[0] == 'Y' && com[2] == 'Y')
            {
                tileData.sprite = Sprites[34];
            }
        }
        if (com[1] == 'Y' && com[3] == 'Y' && com[4] == 'N' && com[6] == 'Y')
        {
            if (com[0] == 'N' && com[5] == 'N')
            {
                tileData.sprite = Sprites[21];
            }
            if (com[0] == 'Y' && com[5] == 'N')
            {
                tileData.sprite = Sprites[26];
            }
            if (com[0] == 'N' && com[5] == 'Y')
            {
                tileData.sprite = Sprites[32];
            }
            if (com[0] == 'Y' && com[5] == 'Y')
            {
                tileData.sprite = Sprites[37];
            }
        }
        if (com[1] == 'Y' && com[3] == 'N' && com[4] == 'Y' && com[6] == 'Y')
        {
            if (com[2] == 'N' && com[7] == 'N')
            {
                tileData.sprite = Sprites[20];
            }
            if (com[2] == 'Y' && com[7] == 'N')
            {
                int a = Random.Range(0, 2);
                tileData.sprite = Sprites[30 + a];
            }
            if (com[2] == 'N' && com[7] == 'Y')
            {
                tileData.sprite = Sprites[25];
            }
            if (com[2] == 'Y' && com[7] == 'Y')
            {
                int a = Random.Range(0, 2);
                tileData.sprite = Sprites[35 + a];
            }
        } 
        if (com[1] == 'N' && com[3] == 'Y' && com[4] == 'Y' && com[6] == 'Y')
        {
            if (com[5] == 'N' && com[7] == 'N')
            {
                tileData.sprite = Sprites[22];
            }
            if (com[5] == 'N' && com[7] == 'Y')
            {
                tileData.sprite = Sprites[33];
            }
            if (com[5] == 'Y' && com[7] == 'N')
            {
                int a = Random.Range(0, 2);
                tileData.sprite = Sprites[27 + a];
            }
            if (com[5] == 'Y' && com[7] == 'Y')
            {
                int a = Random.Range(0, 2);
                tileData.sprite = Sprites[38 + a];
            }
        }
        if (com[1] == 'Y' && com[3] == 'Y' && com[4] == 'Y' && com[6] == 'Y')
        {
            if (com[0] == 'N' && com[2] == 'N' && com[5] == 'N' && com[7] == 'N')
            {
                tileData.sprite = Sprites[40];
            }
            if (com[0] == 'N' && com[2] == 'Y' && com[5] == 'Y' && com[7] == 'Y')
            {
                tileData.sprite = Sprites[54];
            }
            if (com[0] == 'Y' && com[2] == 'N' && com[5] == 'Y' && com[7] == 'Y')
            {
                tileData.sprite = Sprites[55];
            }
            if (com[0] == 'Y' && com[2] == 'Y' && com[5] == 'N' && com[7] == 'Y')
            {
                tileData.sprite = Sprites[56];
            }
            if (com[0] == 'Y' && com[2] == 'Y' && com[5] == 'Y' && com[7] == 'N')
            {
                tileData.sprite = Sprites[57];
            }
            if (com[0] == 'N' && com[2] == 'N' && com[5] == 'Y' && com[7] == 'Y')
            {
                tileData.sprite = Sprites[50];
            }
            if (com[0] == 'N' && com[2] == 'Y' && com[5] == 'N' && com[7] == 'Y')
            {
                tileData.sprite = Sprites[48];
            }
            if (com[0] == 'N' && com[2] == 'Y' && com[5] == 'Y' && com[7] == 'N')
            {
                tileData.sprite = Sprites[53];
            }
            if (com[0] == 'Y' && com[2] == 'N' && com[5] == 'Y' && com[7] == 'N')
            {
                tileData.sprite = Sprites[49];
            }
            if (com[0] == 'Y' && com[2] == 'N' && com[5] == 'N' && com[7] == 'Y')
            {
                int a = Random.Range(0, 2);
                tileData.sprite = Sprites[51 + a];
            }
            if (com[0] == 'Y' && com[2] == 'Y' && com[5] == 'N' && com[7] == 'N')
            {
                int a = Random.Range(0, 2);
                tileData.sprite = Sprites[46 + a];
            }
            if (com[0] == 'N' && com[2] == 'N' && com[5] == 'N' && com[7] == 'Y')
            {
                tileData.sprite = Sprites[45];
            }
            if (com[0] == 'N' && com[2] == 'N' && com[5] == 'Y' && com[7] == 'N')
            {
                tileData.sprite = Sprites[44];
            }
            if (com[0] == 'N' && com[2] == 'Y' && com[5] == 'N' && com[7] == 'N')
            {
                tileData.sprite = Sprites[43];
            }
            if (com[0] == 'Y' && com[2] == 'N' && com[5] == 'N' && com[7] == 'N')
            {
                int a = Random.Range(0, 2);
                tileData.sprite = Sprites[41 + a];
            }
            if (com[0] == 'Y' && com[2] == 'Y' && com[5] == 'Y' && com[7] == 'Y')
            {
                int a = Random.Range(0, 7);
                tileData.sprite = Sprites[58 + a];
            }
        }
    }

    bool haveIt(ITilemap tilemap, Vector3Int pos)
    {
        return (tilemap.GetTile(pos) == this);
    }
}
