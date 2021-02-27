using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Dust
{
    protected override Sprite[] sprites => VFXManager.Instance.SpriteFire;
}
