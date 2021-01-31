using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TrangThaiTrangBiVuKhi
{
    Tudo,
    TrongTui,
    DangTrangBi,
}

public enum LevelWeapon
{
    Common,
    Epic,
    Rare,
    VeryRare,
    Legendary,
}
[RequireComponent(typeof(Reward))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(HienTenVuKhi))]
public abstract class Weapon : MonoBehaviour
{
    // Không thể bắn khi nằm trong túi
    [SerializeField] protected string nameOfWeapon;
    [SerializeField] protected LevelWeapon type;

    public LevelWeapon TypeOfWeapon => type;
    public abstract string NameOfWeapon
    {
        get;
    }
    [HideInInspector] public TrangThaiTrangBiVuKhi TrangThai;
    [HideInInspector] public Entity Host;
    [HideInInspector] public Enemy Target;

    protected Reward reward => GetComponent<Reward>();
    protected SpriteRenderer render => GetComponent<SpriteRenderer>();
    protected HienTenVuKhi hientenvukhi => GetComponent<HienTenVuKhi>();
    public abstract Vector3 viTriRaDan
    {
        get;
    }
    protected virtual void Start()
    {
    }
    protected abstract bool ReadyToAttack
    {
        get;
    }
    public abstract void Attack();
    public void ChangEQuip(Entity host, TrangThaiTrangBiVuKhi trangthai)
    {
        if (trangthai == TrangThaiTrangBiVuKhi.TrongTui)
        {
            Host = host;
            OnBoVaoTui();
        }
        if (trangthai == TrangThaiTrangBiVuKhi.Tudo)
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
        if (trangthai == TrangThaiTrangBiVuKhi.DangTrangBi)
        {
            Host = host;
            transform.parent = host.transform;
            transform.position = host.PositionSpawnWeapon;
            if (GetComponent<PositionControl>() != null)
            {
                Destroy(GetComponent<PositionControl>());
            }
            ThongBao.Nen("Đã trang bị " + "<color=" + getColorNameByLevelWeapon(TypeOfWeapon) + ">" + nameOfWeapon + "</color>");
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
