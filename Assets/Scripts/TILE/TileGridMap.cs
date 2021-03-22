using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "New Flower", menuName = "Tiles/Flower")]
public class TileGridMap : Tile
{
    public bool isTrigger = true;
    public int Offset_Sorting = 0;
    public Vector2 Offset_Position = new Vector2(0.5f, 0.5f);
    public string layerSort;
    public Vector2 Size_Collider = new Vector2(1, 1);

    private GameObject goes;
    private Vector3Int pos;
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (go != null)
        {
            goes = go;
            pos = position;
            SpriteRenderer render = go.GetComponent<SpriteRenderer>();
            go.transform.position = position + (Vector3)Offset_Position;
            render.sprite = sprite;
            render.sortingLayerName = layerSort;
            render.sortingOrder = (int)(-go.transform.position.y * 10) - Offset_Sorting;
            BoxCollider2D col = go.GetComponent<BoxCollider2D>();
            col.offset = new Vector2(0.5f, 0.5f) - Offset_Position;
            col.size = Size_Collider;
            col.isTrigger = isTrigger;
        }
        return base.StartUp(position, tilemap, go);
    }
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
    }

    private void OnValidate()
    {
        if (goes != null)
        goes.transform.position = pos + (Vector3)Offset_Position;
    }
}
