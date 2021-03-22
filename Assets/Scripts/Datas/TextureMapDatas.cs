using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Textures", menuName = "TextureMap/new Textures")]
public class TextureMapDatas : ScriptableObject
{
    [SerializeField]
    CodeMap codejoin = CodeMap.Map1;
    [SerializeField]
    TypeRound typeRound;
    public Texture2D[] textures;

    public bool EqualCodes(CodeMap c, TypeRound t)
    {
        return c == codejoin && t == typeRound;
    }

    public Texture2D GetTexture2D()
    {
        if (textures == null || textures.Length == 0)
        {
            return null;
        }
        else
        {
            return textures[Random.Range(0, textures.Length)];
        }
    }
}
