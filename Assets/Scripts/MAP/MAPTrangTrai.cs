using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class MAPTrangTrai : MAPController
{
    protected override PlayerController CreatePlayer()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        for (int i = 0; i < players.Length; i++)
        {
            Destroy(players[i].gameObject);
        }
        PlayerController player = base.CreatePlayer();
        return player;
    }
}
