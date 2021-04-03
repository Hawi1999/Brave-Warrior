using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Code", menuName = "Language/Sprites")]
public class LangSprite : ScriptableObject
{
    public string CODE => name;
    public Sprite VietNam;
    public Sprite English;
}
