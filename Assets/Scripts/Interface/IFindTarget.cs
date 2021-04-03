using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeTarget
{
    Enemy,
    Player,
    Object
}

public interface IFindTarget: ICameraTarget
{
    TypeTarget typeTarget
    {
        get;
    }
    Vector2 size { get;}
    void OnTargetFound(Entity host, IFindTarget target);
    bool IsForFind {get; }
}
