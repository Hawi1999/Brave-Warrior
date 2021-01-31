using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DataMap))]
[RequireComponent(typeof(DrawMap))]
public class MAP_GamePlay : MAPController
{
    [SerializeField]
    private ShowHPPlayer TT;
    public override void SetPlayerPositionInMap()
    {
        SetPlayerPosDefault();
    }

    protected override PlayerController CreatePlayer()
    {
        if (TT == null)
        {
            Debug.Log("Không có Prefab hiển thị máu Player");
        } else 
        Instantiate(TT, GameController.CanvasMain.transform);
        return null;
    }
}
