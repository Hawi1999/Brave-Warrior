using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New MultiSprites", menuName = "Tiles/MultiSprites")]
public class MultiSpritesTile : Tile
{
    [SerializeField] Sprite[] sprites;
    // Start is called before the first frame update
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        tileData.sprite = GetRandomSprite();
    }

    public Sprite GetRandomSprite()
    {
        if (sprites == null || sprites.Length == 0)
        {
            return sprite;
        }
        return sprites[Random.Range(0, sprites.Length)];
    }

}
