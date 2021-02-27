using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DrawMap))]
public class MAP_GamePlay : MAPController
{
    public override void SetPlayerPositionInMap()
    {
        SetPlayerPosDefault();
    }

    protected override PlayerController CreatePlayer()
    {
        return null;
    }
}
