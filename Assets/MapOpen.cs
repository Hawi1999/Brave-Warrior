using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOpen : MAPController
{

    [SerializeField]
    private ShowHPPlayer TT;
    protected override PlayerController CreatePlayer()
    {
        if (ParentGamePlay.Instance != null)
        {
            Destroy(ParentGamePlay.Instance.gameObject);
        }
        Instantiate(new GameObject("ParentGamePlay")).AddComponent<ParentGamePlay>();
        PlayerController player = base.CreatePlayer();
        player.transform.parent = ParentGamePlay.Instance;
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
        else if(PlayerController.PlayerCurrent.HasWeapon)
        {
            GameController.Instance.LoadScene(scene);
        } else
        {
            Notification.ReMind("Bạn cần vũ khí để đi tiếp");
        }
    }
}
