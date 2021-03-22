using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public enum TypeTile
{
    Wall = 1,
    Current = 2,
    Land = 3
}

[CreateAssetMenu(fileName = "New TileData", menuName = "Data/Tile")]
public class TileDatas : ScriptableObject
{
    [Header("Info (Can't Edit).")]
    public string CodeColor;
    public CodeMap codeMap = CodeMap.Map1;
    [Space]
    [SerializeField] protected Tile tile;
    [SerializeField] protected Color colorConnect;
    public TypeTile type = TypeTile.Land;

    private void OnValidate()
    {
        CodeColor = ColorUtility.ToHtmlStringRGB(colorConnect);
    }

    public virtual Tile GetTile()
    {
        return tile;
    }

    public bool EqualsColor(Color color)
    {
        return (ColorUtility.ToHtmlStringRGB(colorConnect) == ColorUtility.ToHtmlStringRGB(color));
    }

}
