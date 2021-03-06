﻿using System.Collections;
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
        if (go != null)
        {
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
            go.GetComponent<SpriteRenderer>().sprite = getSprite(com);
        }
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
        tileData.sprite = getSprite(com);
        tileData.gameObject.GetComponent<SpriteRenderer>();
    }

    private Sprite getSprite(string com)
    {
        if (com[1] == 'N' && com[3] == 'N' && com[4] == 'N' && com[6] == 'N')
        {
            return Sprites[0];
        }
        if (com[1] == 'Y' && com[3] == 'N' && com[4] == 'N' && com[6] == 'N')
        {
            return Sprites[1];
        }
        if (com[1] == 'N' && com[3] == 'Y' && com[4] == 'N' && com[6] == 'N')
        {
            int a = Random.Range(0, 2);
            return Sprites[4 + a];
        }
        if (com[1] == 'N' && com[3] == 'N' && com[4] == 'Y' && com[6] == 'N')
        {
            int a = Random.Range(0, 1);
            return Sprites[2 + a];
        }
        if (com[1] == 'N' && com[3] == 'N' && com[4] == 'N' && com[6] == 'Y')
        {
            return Sprites[6];
        }
        if (com[1] == 'Y' && com[3] == 'Y' && com[4] == 'N' && com[6] == 'N')
        {
            if (com[0] == 'Y')
            {
                return Sprites[7];
            }
            else
            {
                int a = Random.Range(0, 2);
                return Sprites[12 + a];
            }
        }
        if (com[1] == 'Y' && com[3] == 'N' && com[4] == 'Y' && com[6] == 'N')
        {
            if (com[2] == 'Y')
            {
                int a = Random.Range(0, 2);
                return Sprites[8 + a];
            }
            else
            {
                return Sprites[14];
            }
        }
        if (com[1] == 'N' && com[3] == 'N' && com[4] == 'Y' && com[6] == 'Y')
        {
            if (com[7] == 'Y')
            {
                return Sprites[11];
            }
            else
            {
                return Sprites[17];
            }
        }
        if (com[1] == 'N' && com[3] == 'Y' && com[4] == 'N' && com[6] == 'Y')
        {
            if (com[5] == 'Y')
            {
                return Sprites[10];
            }
            else
            {
                int a = Random.Range(0, 2);
                return Sprites[15 + a];
            }
        }
        if (com[1] == 'Y' && com[3] == 'N' && com[4] == 'N' && com[6] == 'Y')
        {
            return Sprites[65];
        }
        if (com[1] == 'N' && com[3] == 'Y' && com[4] == 'Y' && com[6] == 'N')
        {
            return Sprites[66];
        }
        if (com[1] == 'Y' && com[3] == 'Y' && com[4] == 'Y' && com[6] == 'N')
        {
            if (com[0] == 'N' && com[2] == 'N')
            {
                int a = Random.Range(0, 2);
                return Sprites[18 + a];
            }
            if (com[0] == 'N' && com[2] == 'Y')
            {
                int a = Random.Range(0, 2);
                return Sprites[23 + a];
            }
            if (com[0] == 'Y' && com[2] == 'N')
            {
                return Sprites[29];
            }
            if (com[0] == 'Y' && com[2] == 'Y')
            {
                return Sprites[34];
            }
        }
        if (com[1] == 'Y' && com[3] == 'Y' && com[4] == 'N' && com[6] == 'Y')
        {
            if (com[0] == 'N' && com[5] == 'N')
            {
                return Sprites[21];
            }
            if (com[0] == 'Y' && com[5] == 'N')
            {
                return Sprites[26];
            }
            if (com[0] == 'N' && com[5] == 'Y')
            {
                return Sprites[32];
            }
            if (com[0] == 'Y' && com[5] == 'Y')
            {
                return Sprites[37];
            }
        }
        if (com[1] == 'Y' && com[3] == 'N' && com[4] == 'Y' && com[6] == 'Y')
        {
            if (com[2] == 'N' && com[7] == 'N')
            {
                return Sprites[20];
            }
            if (com[2] == 'Y' && com[7] == 'N')
            {
                int a = Random.Range(0, 2);
                return Sprites[30 + a];
            }
            if (com[2] == 'N' && com[7] == 'Y')
            {
                return Sprites[25];
            }
            if (com[2] == 'Y' && com[7] == 'Y')
            {
                int a = Random.Range(0, 2);
                return Sprites[35 + a];
            }
        } 
        if (com[1] == 'N' && com[3] == 'Y' && com[4] == 'Y' && com[6] == 'Y')
        {
            if (com[5] == 'N' && com[7] == 'N')
            {
                return Sprites[22];
            }
            if (com[5] == 'N' && com[7] == 'Y')
            {
                return Sprites[33];
            }
            if (com[5] == 'Y' && com[7] == 'N')
            {
                int a = Random.Range(0, 2);
                return Sprites[27 + a];
            }
            if (com[5] == 'Y' && com[7] == 'Y')
            {
                int a = Random.Range(0, 2);
                return Sprites[38 + a];
            }
        }
        if (com[1] == 'Y' && com[3] == 'Y' && com[4] == 'Y' && com[6] == 'Y')
        {
            if (com[0] == 'N' && com[2] == 'N' && com[5] == 'N' && com[7] == 'N')
            {
                return Sprites[40];
            }
            if (com[0] == 'N' && com[2] == 'Y' && com[5] == 'Y' && com[7] == 'Y')
            {
                return Sprites[54];
            }
            if (com[0] == 'Y' && com[2] == 'N' && com[5] == 'Y' && com[7] == 'Y')
            {
                return Sprites[55];
            }
            if (com[0] == 'Y' && com[2] == 'Y' && com[5] == 'N' && com[7] == 'Y')
            {
                return Sprites[56];
            }
            if (com[0] == 'Y' && com[2] == 'Y' && com[5] == 'Y' && com[7] == 'N')
            {
                return Sprites[57];
            }
            if (com[0] == 'N' && com[2] == 'N' && com[5] == 'Y' && com[7] == 'Y')
            {
                return Sprites[50];
            }
            if (com[0] == 'N' && com[2] == 'Y' && com[5] == 'N' && com[7] == 'Y')
            {
                return Sprites[48];
            }
            if (com[0] == 'N' && com[2] == 'Y' && com[5] == 'Y' && com[7] == 'N')
            {
                return Sprites[53];
            }
            if (com[0] == 'Y' && com[2] == 'N' && com[5] == 'Y' && com[7] == 'N')
            {
                return Sprites[49];
            }
            if (com[0] == 'Y' && com[2] == 'N' && com[5] == 'N' && com[7] == 'Y')
            {
                int a = Random.Range(0, 2);
                return Sprites[51 + a];
            }
            if (com[0] == 'Y' && com[2] == 'Y' && com[5] == 'N' && com[7] == 'N')
            {
                int a = Random.Range(0, 2);
                return Sprites[46 + a];
            }
            if (com[0] == 'N' && com[2] == 'N' && com[5] == 'N' && com[7] == 'Y')
            {
                return Sprites[45];
            }
            if (com[0] == 'N' && com[2] == 'N' && com[5] == 'Y' && com[7] == 'N')
            {
                return Sprites[44];
            }
            if (com[0] == 'N' && com[2] == 'Y' && com[5] == 'N' && com[7] == 'N')
            {
                return Sprites[43];
            }
            if (com[0] == 'Y' && com[2] == 'N' && com[5] == 'N' && com[7] == 'N')
            {
                int a = Random.Range(0, 2);
                return Sprites[41 + a];
            }
            if (com[0] == 'Y' && com[2] == 'Y' && com[5] == 'Y' && com[7] == 'Y')
            {
                int a = Random.Range(0, 7);
                return Sprites[58 + a];
            }
        }
        return null;

    }

    bool haveIt(ITilemap tilemap, Vector3Int pos)
    {
        return (tilemap.GetTile(pos) == this);
    }
}
