using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOpen : MAPController
{
    protected override PlayerController CreatePlayer()
    {
        PlayerController player = base.CreatePlayer();
        DontDestroyOnLoad(player);
        GunBase gun = WeaponManager.GetWeaponByName("Gun K1") as GunBase;
        gun = Instantiate(gun);
        player.TrangBi(gun);
        return player;
    }

    public override void LoadScene(SceneGame scene)
    {
        if (PlayerController.Instance.HasWeapon && scene == SceneGame.Map1_0)
        {
            GameController.Instance.LoadScene(scene);
        } else
        {
            ThongBao.NhacNho("Bạn cần vũ khí để đi tiếp");
        }
        if (scene == SceneGame.TrangTrai)
        {
            Destroy(PlayerController.Instance);
            GameController.Instance.LoadScene(scene);
        }
    }
}
