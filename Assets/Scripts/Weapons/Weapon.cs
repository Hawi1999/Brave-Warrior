using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum WeaponStatus
{
    Free,
    Pocket,
    Equiping,
}

public enum LevelWeapon
{
    Common,
    Epic,
    Rare,
    VeryRare,
    Legendary,
}
[RequireComponent(typeof(RewardWeapon))]
[RequireComponent(typeof(HienTenVuKhi))]
public abstract class Weapon : MonoBehaviour
{
    // Không thể bắn khi nằm trong túi
    [SerializeField] Sprite Picture;
    [SerializeField] protected string nameOfWeapon;
    [SerializeField] protected LevelWeapon type;
    [SerializeField] protected int _SatThuong;


    public LevelWeapon TypeOfWeapon => type;

    public abstract string GetNameOfWeapon();

    [HideInInspector] public virtual int SatThuong => _SatThuong;
    [HideInInspector] public WeaponStatus TrangThai;
    [HideInInspector] public Entity Host;
    [HideInInspector] public Enemy Target;
    public virtual SpriteRenderer render
    {
        get; set;
    }


    public UnityAction OnAttacked;

    protected Reward reward => GetComponent<Reward>();
    protected HienTenVuKhi hientenvukhi => GetComponent<HienTenVuKhi>();
    public abstract Vector3 PositionStartAttack
    {
        get;
    }
    protected abstract bool ReadyToAttack
    {
        get;
    }

    protected virtual void Start()
    {
        render = Instantiate(new GameObject("Picture"), transform).AddComponent<SpriteRenderer>();
        render.sprite = Picture;
        render.sortingLayerName = "Skin";
        render.sortingOrder = 15;
    }
    public abstract bool Attack(DamageData damageData);
    public void ChangEQuip(Entity host, WeaponStatus trangthai)
    {
        if (trangthai == WeaponStatus.Pocket)
        {
            Host = host;
            OnBoVaoTui();
        }
        if (trangthai == WeaponStatus.Free)
        {
            if (Host != null)
            {
                transform.position = Host.transform.position;
                transform.rotation = Quaternion.identity;
                render.flipX = false;
                render.flipY = false;
            }
            transform.parent = TransformInstanceOnLoad.getTransform();
            Host = null;
            ChooseReward chooseReward = host.GetComponent<ChooseReward>();
            if (chooseReward != null)
            {
                chooseReward.Remove(reward);
            }
            OnTuDo();
        }
        if (trangthai == WeaponStatus.Equiping)
        {
            Host = host;
            transform.parent = host.transform;
            transform.position = host.PositionSpawnWeapon;
            if (GetComponent<PositionControl>() != null)
            {
                Destroy(GetComponent<PositionControl>());
            }
            Notification.NoticeBelow("Đã trang bị " + "<color=" + getColorNameByLevelWeapon(TypeOfWeapon) + ">" + nameOfWeapon + "</color>");
            OnEquip();
        }
        TrangThai = trangthai;
    }
    public virtual void OnBoVaoTui()
    {
        
    }
    public virtual void OnTuDo()
    {

    }
    public virtual void OnEquip()
    {

    }

    public static string getColorNameByLevelWeapon(LevelWeapon type)
    {
        switch (type)
        {
            case LevelWeapon.Common:
                return "green";
            case LevelWeapon.Epic:
                return "blue";
            case LevelWeapon.Rare:
                return "yellow";
            case LevelWeapon.VeryRare:
                return "red";
            case LevelWeapon.Legendary:
                return "purple";
            default:
                return "green";
        }
    }

    public static Color getColorByLevelWeapon(LevelWeapon type)
    {
        switch (type)
        {
            case LevelWeapon.Common:
                return Color.green;
            case LevelWeapon.Epic:
                return Color.blue;
            case LevelWeapon.Rare:
                return Color.yellow;
            case LevelWeapon.VeryRare:
                return Color.red;
            case LevelWeapon.Legendary:
                return new Color(1,0,1);
            default:
                return Color.green;
        }
    }
}
