using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeTarget
{
    Enemy,
    Player,
}

public interface IFindTarget: ICameraTarget
{
    TypeTarget typeTarget
    {
        get;
    }
    bool IsForFind
    {
        get;
    }
    Vector2 size { get;}
}
