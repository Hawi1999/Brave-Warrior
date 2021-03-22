using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundChest : RoundBase
{
    public override void SetUp(RoundData roundData)
    {
        base.SetUp(roundData);
        ChestManager.SpawnReWardChest(roundData.colorChest, Data.typeChest, (Vector2)Data.position);
    }
}
