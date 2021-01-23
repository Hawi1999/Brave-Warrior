using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "New Tree", menuName = "Tiles/Tree")]
public class TreeTile : Tile
{
    public int Pivot_Offset = 0;
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (go != null)
        {
            go.GetComponent<SpriteRenderer>().sortingOrder = -(int)(go.transform.position.y * 10f) - Pivot_Offset;
            go.GetComponent<SpriteRenderer>().sortingLayerName = "Current";
            go.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        return base.StartUp(position, tilemap, go);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
    }
}
