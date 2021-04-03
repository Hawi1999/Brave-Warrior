using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOpen : MAPController
{
    public Transform PositionSpawnChestStart;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        ChestManager.SpawnStartChest(ColorChest.Silver, CodeMap.Map1, PositionSpawnChestStart ? PositionSpawnChestStart.position : Vector3.zero);
    }

    protected override PlayerController CreatePlayer()
    {
        if (ParentGamePlay.Instance != null)
        {
            Destroy(ParentGamePlay.Instance.gameObject);
        }
        Instantiate(new GameObject("ParentGamePlay")).AddComponent<ParentGamePlay>();
        PlayerController player = base.CreatePlayer();
        PlayerController.PlayerTakeBuff.Reset();
        player.transform.parent = ParentGamePlay.Instance;
        return player;
    }

    public override void LoadScene(string scene)
    {
        
        if (scene == "TrangTrai")
        {
            if (PlayerController.PlayerCurrent.HasWeapon)
            {
                Notification.AreYouSure(Languages.getString("BanSe") +" <color=red> " + Languages.getString("MatVuKhiHienTai") + "</color> " + Languages.getString("NeuQuayVeTrangTrai") + "!", () => GameController.Instance.LoadScene(scene));

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
            Notification.ReMind(Languages.getString("BanCanVuKhiDeDiTiep"));
        }
    }
}
