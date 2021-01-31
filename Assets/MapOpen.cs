using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOpen : MAPController
{

    [SerializeField]
    private ShowHPPlayer TT;
    protected override PlayerController CreatePlayer()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        for (int i = 0; i < players.Length; i++)
        {
            Destroy(players[i].gameObject);
        }
        PlayerController player = base.CreatePlayer();
        DontDestroyOnLoad(player);
        GunBase gun = WeaponManager.GetWeaponByName("Gun K1") as GunBase;
        gun = Instantiate(gun);
        player.TrangBi(gun);
        if (TT == null)
        {
            Debug.Log("Không có Prefab hiển thị máu Player");
        }
        else
            Instantiate(TT, GameController.CanvasMain.transform);
        return player;
    }

    public override void LoadScene(string scene)
    {
        
        if (scene == "TrangTrai")
        {
            GameController.Instance.LoadScene(scene);
        }
        else if(PlayerController.Instance.HasWeapon)
        {
            GameController.Instance.LoadScene(scene);
        } else
        {
            ThongBao.NhacNho("Bạn cần vũ khí để đi tiếp");
        }
    }
}
