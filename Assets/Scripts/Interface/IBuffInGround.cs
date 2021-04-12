using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffInGround
{
    void OnHostTake(Entity entity);
    Sprite Sprite { get; }
    string name { get; }
}
