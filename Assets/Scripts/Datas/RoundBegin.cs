using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoundBegin : RoundBase, IBattle
{
    public void OnGameStarted()
    {
        EntityManager.Instance.SpawnPlayer();
        OpenAllDoor();
    }

    public void OnGameEnded()
    {
        
    }
}
