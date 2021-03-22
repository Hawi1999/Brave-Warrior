using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "New TileData", menuName = "Data/MultiTile")]
public class MultiTileDatas : TileDatas
{
    [SerializeField] Tile[] MoreTile;

    public override Tile GetTile()
    {
        if (MoreTile == null)
        {
            return this.tile;
        }
        int le = MoreTile.Length;
        if (tile != null)
        {
            le += 1;
        }
        Tile[] tiles = new Tile[le];
        MoreTile.CopyTo(tiles, 0);
        if (tile != null)
        {
            tiles[le - 1] = this.tile;
        }
        return tiles[Random.Range(0, le)];
    }
}
