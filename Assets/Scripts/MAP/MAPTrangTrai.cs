using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class MAPTrangTrai : MAPController
{
    protected override PlayerController CreatePlayer()
    {
        if (ParentGamePlay.Instance != null)
        {
            Destroy(ParentGamePlay.Instance.gameObject);
        }
        PlayerController player = base.CreatePlayer();
        return player;
    }
}
