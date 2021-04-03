using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoundBegin : RoundBase, IBattle
{
    public void OnSceneStarted()
    {
        EntityManager.Instance.SpawnPlayer(PlayerController.PlayerCurrent);
        OpenAllDoor();
    }

    public void OnSceneEnded()
    {
        
    }
}
