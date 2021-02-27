using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOpen : MAPController
{
    protected override PlayerController CreatePlayer()
    {
        if (ParentGamePlay.Instance != null)
        {
            Destroy(ParentGamePlay.Instance.gameObject);
        }
        Instantiate(new GameObject("ParentGamePlay")).AddComponent<ParentGamePlay>();
        PlayerController player = base.CreatePlayer();
        player.transform.parent = ParentGamePlay.Instance;
        return player;
    }

    public override void LoadScene(string scene)
    {
        
        if (scene == "TrangTrai")
        {
            if (PlayerController.PlayerCurrent.WeaponCurrent != null)
            {
                Notification.AreYouSure("Bạn sẽ <color=red>mất vũ khí hiện tại</color> nếu quay về Trang Trại!", () => GameController.Instance.LoadScene(scene));
            } else
            {
                GameController.Instance.LoadScene(scene);
            }
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
