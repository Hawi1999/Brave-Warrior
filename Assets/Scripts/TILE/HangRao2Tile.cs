using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Fence", menuName = "Tiles/Fence 246")]
public class HangRao2Tile : Tile
{
    [SerializeField]
    private Sprite[] LSprites;
    [SerializeField]
    private int Offset_Sorting;
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (go != null)
        {
            SpriteRenderer render = go.GetComponent<SpriteRenderer>();
            render.sprite = GetSpriteTile(position, tilemap);
            render.sortingOrder = (int)(render.transform.position.y * -10f) - Offset_Sorting;
            render.sortingLayerName = "Current";
        }
        return base.StartUp(position, tilemap, go);
    }
    private Sprite GetSpriteTile(Vector3Int position, ITilemap tilemap)
    {
        Vector2Int[] posr = new Vector2Int[3];
        posr[0] = new Vector2Int(0, -1);
        posr[1] = new Vector2Int(-1, 0);
        posr[2] = new Vector2Int(1, 0);
        string s = string.Empty;
        for (int y = 0; y < 3; y++)
        {
            Vector3Int pos = new Vector3Int(position.x + posr[y].x, position.y + posr[y].y, position.z);
            if (haveIt(tilemap, pos))
            {
                s += "Y";
            } else
            {
                s += "N";
            }
        }
        if (s[0] == 'Y')
            if (s[1] == 'Y')
                if (s[2] == 'Y')
                    return LSprites[1]; //YYY
                else
                    return LSprites[4]; //YYN
            else
            if (s[2] == 'Y')
                return LSprites[3]; //YNY
            else
                return LSprites[2]; //YNN
        else
        if (s[1] == 'Y')
            if (s[2] == 'Y')
                return LSprites[5]; //NYY
            else
                return LSprites[7]; //NYN
        else
        if (s[2] == 'Y')
            return LSprites[6]; //NNY
        else return LSprites[0]; //NNN
    }
    bool haveIt(ITilemap tilemap, Vector3Int pos)
    {
        return (tilemap.GetTile(pos) == this);
    }
}
