using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "New Lane", menuName = "Tiles/Lane")]
public class DuongMon : Tile
{
    [SerializeField]
    private Sprite[] DuongMonSprites;
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
        string composition = string.Empty;
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (x == 0 && y == 0)
                    continue;
                Vector3Int pos = new Vector3Int(position.x + x, position.y + y, position.z);
                if (haveIt(tilemap, pos))
                {
                    composition += "Y";
                } else
                {
                    composition += "N";
                }
            }
        }
        if (composition[1] == 'N' && composition[3] == 'N' && composition[4] == 'N' && composition[6] == 'N')
        {
            tileData.sprite = sprite;
        } 
        if (composition[1] == 'Y' && composition[3] == 'N' && composition[4] == 'N' && composition[6] == 'N')
        {
            tileData.sprite = DuongMonSprites[0];
        } 
        if (composition[1] == 'N' && composition[3] == 'Y' && composition[4] == 'N' && composition[6] == 'N')
        {
            tileData.sprite = DuongMonSprites[1];
        } 
        if (composition[1] == 'N' && composition[3] == 'N' && composition[4] == 'Y' && composition[6] == 'N')
        {
            tileData.sprite = DuongMonSprites[2];
        } 
        if (composition[1] == 'N' && composition[3] == 'N' && composition[4] == 'N' && composition[6] == 'Y')
        {
            tileData.sprite = DuongMonSprites[3];
        }
        if (composition[1] == 'Y' && composition[3] == 'Y' && composition[4] == 'N' && composition[6] == 'N')
        {
            if (composition[0] == 'Y')
            {
                tileData.sprite = DuongMonSprites[5];
            } else
            {
                tileData.sprite = DuongMonSprites[4];
            }
        }
        if (composition[1] == 'Y' && composition[3] == 'N' && composition[4] == 'Y' && composition[6] == 'N')
        {
            if (composition[2] == 'Y')
            {
                tileData.sprite = DuongMonSprites[7];
            }
            else
            {
                tileData.sprite = DuongMonSprites[6];
            }
        }
        if (composition[1] == 'N' && composition[3] == 'N' && composition[4] == 'Y' && composition[6] == 'Y')
        {
            if (composition[7] == 'Y')
            {
                tileData.sprite = DuongMonSprites[9];
            }
            else
            {
                tileData.sprite = DuongMonSprites[8];
            }
        }
        if (composition[1] == 'N' && composition[3] == 'Y' && composition[4] == 'N' && composition[6] == 'Y')
        {
            if (composition[5] == 'Y')
            {
                tileData.sprite = DuongMonSprites[11];
            }
            else
            {
                tileData.sprite = DuongMonSprites[10];
            }
        }
        if (composition[1] == 'Y' && composition[3] == 'N' && composition[4] == 'N' && composition[6] == 'Y')
        {
            tileData.sprite = DuongMonSprites[47];
        }
        if (composition[1] == 'N' && composition[3] == 'Y' && composition[4] == 'Y' && composition[6] == 'N')
        {
            tileData.sprite = DuongMonSprites[48];
        }
        if (composition[1] == 'Y' && composition[3] == 'Y' && composition[4] == 'Y' && composition[6] == 'N')
        {
            if (composition[0] == 'N' && composition[2] == 'N')
            {
                tileData.sprite = DuongMonSprites[12];
            }
            if (composition[0] == 'N' && composition[2] == 'Y')
            {
                tileData.sprite = DuongMonSprites[13];
            }
            if (composition[0] == 'Y' && composition[2] == 'N')
            {
                tileData.sprite = DuongMonSprites[14];
            }
            if (composition[0] == 'Y' && composition[2] == 'Y')
            {
                tileData.sprite = DuongMonSprites[15];
            }
        }
        if (composition[1] == 'Y' && composition[3] == 'Y' && composition[4] == 'N' && composition[6] == 'Y')
        {
            if (composition[0] == 'N' && composition[5] == 'N')
            {
                tileData.sprite = DuongMonSprites[16];
            }
            if (composition[0] == 'Y' && composition[5] == 'N')
            {
                tileData.sprite = DuongMonSprites[18];
            }
            if (composition[0] == 'N' && composition[5] == 'Y')
            {
                tileData.sprite = DuongMonSprites[17];
            }
            if (composition[0] == 'Y' && composition[5] == 'Y')
            {
                tileData.sprite = DuongMonSprites[19];
            }
        }
        if (composition[1] == 'Y' && composition[3] == 'N' && composition[4] == 'Y' && composition[6] == 'Y')
        {
            if (composition[2] == 'N' && composition[7] == 'N')
            {
                tileData.sprite = DuongMonSprites[20];
            }
            if (composition[2] == 'Y' && composition[7] == 'N')
            {
                tileData.sprite = DuongMonSprites[21];
            }
            if (composition[2] == 'N' && composition[7] == 'Y')
            {
                tileData.sprite = DuongMonSprites[22];
            }
            if (composition[2] == 'Y' && composition[7] == 'Y')
            {
                tileData.sprite = DuongMonSprites[23];
            }
        }
        if (composition[1] == 'N' && composition[3] == 'Y' && composition[4] == 'Y' && composition[6] == 'Y')
        {
            if (composition[5] == 'N' && composition[7] == 'N')
            {
                tileData.sprite = DuongMonSprites[24];
            }
            if (composition[5] == 'N' && composition[7] == 'Y')
            {
                tileData.sprite = DuongMonSprites[25];
            }
            if (composition[5] == 'Y' && composition[7] == 'N')
            {
                tileData.sprite = DuongMonSprites[26];
            }
            if (composition[5] == 'Y' && composition[7] == 'Y')
            {
                tileData.sprite = DuongMonSprites[27];
            }
        }
        if (composition[1] == 'Y' && composition[3] == 'Y' && composition[4] == 'Y' && composition[6] == 'Y')
        {
            if(composition[0] == 'N' && composition[2] == 'N' && composition[5] == 'N' && composition[7] == 'N')
            {
                tileData.sprite = DuongMonSprites[28];
            }
            if (composition[0] == 'N' && composition[2] == 'Y' && composition[5] == 'Y' && composition[7] == 'Y')
            {
                tileData.sprite = DuongMonSprites[29];
            }
            if (composition[0] == 'Y' && composition[2] == 'N' && composition[5] == 'Y' && composition[7] == 'Y')
            {
                tileData.sprite = DuongMonSprites[30];
            }
            if (composition[0] == 'Y' && composition[2] == 'Y' && composition[5] == 'N' && composition[7] == 'Y')
            {
                tileData.sprite = DuongMonSprites[31];
            }
            if (composition[0] == 'Y' && composition[2] == 'Y' && composition[5] == 'Y' && composition[7] == 'N')
            {
                tileData.sprite = DuongMonSprites[32];
            }
            if (composition[0] == 'N' && composition[2] == 'N' && composition[5] == 'Y' && composition[7] == 'Y')
            {
                tileData.sprite = DuongMonSprites[33];
            }
            if (composition[0] == 'N' && composition[2] == 'Y' && composition[5] == 'N' && composition[7] == 'Y')
            {
                tileData.sprite = DuongMonSprites[34];
            }
            if (composition[0] == 'N' && composition[2] == 'Y' && composition[5] == 'Y' && composition[7] == 'N')
            {
                tileData.sprite = DuongMonSprites[35];
            }
            if (composition[0] == 'Y' && composition[2] == 'N' && composition[5] == 'Y' && composition[7] == 'N')
            {
                tileData.sprite = DuongMonSprites[36];
            }
            if (composition[0] == 'Y' && composition[2] == 'N' && composition[5] == 'N' && composition[7] == 'Y')
            {
                tileData.sprite = DuongMonSprites[37];
            }
            if (composition[0] == 'Y' && composition[2] == 'Y' && composition[5] == 'N' && composition[7] == 'N')
            {
                tileData.sprite = DuongMonSprites[38];
            }
            if (composition[0] == 'N' && composition[2] == 'N' && composition[5] == 'N' && composition[7] == 'Y')
            {
                tileData.sprite = DuongMonSprites[39];
            }
            if (composition[0] == 'N' && composition[2] == 'N' && composition[5] == 'Y' && composition[7] == 'N')
            {
                tileData.sprite = DuongMonSprites[40];
            }
            if (composition[0] == 'N' && composition[2] == 'Y' && composition[5] == 'N' && composition[7] == 'N')
            {
                tileData.sprite = DuongMonSprites[41];
            }
            if (composition[0] == 'Y' && composition[2] == 'N' && composition[5] == 'N' && composition[7] == 'N')
            {
                tileData.sprite = DuongMonSprites[42];
            }
            if (composition[0] == 'Y' && composition[2] == 'Y' && composition[5] == 'Y' && composition[7] == 'Y')
            {
                int a = Random.Range(0, 4);
                tileData.sprite = DuongMonSprites[43 + a];
            }
        }
    }
    bool haveIt(ITilemap tilemap, Vector3Int pos)
    {
        return tilemap.GetTile(pos) == this;
    }
}
