using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSeller : NPC
{

    [SerializeField] UIChooseWeaponSeller ui;
    public override void TakeManipulation(PlayerController host)
    {
        base.TakeManipulation(host);
        ui.Show();
    }
    private void Awake()
    {
        if (ui != null)
        {
            ui.OnDeShow += DeGeting;
        }
    }
}
