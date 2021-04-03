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
[RequireComponent(typeof(ShowName))]
public abstract class Weapon : MonoBehaviour, IShowName
{
    #region Editor In Inpector
    // Không thể bắn khi nằm trong túi
    [SerializeField] Sprite Picture;
    [SerializeField] protected SpriteRenderer _render;
    [SerializeField] protected string nameOfWeapon;
    [SerializeField] protected LevelWeapon type;
    [SerializeField] protected int _SatThuong;

    #endregion
    public LevelWeapon TypeOfWeapon => type;

    public abstract string GetNameOfWeapon();

    [HideInInspector] public virtual int SatThuong => _SatThuong;
    [HideInInspector] public WeaponStatus TrangThai;
    [HideInInspector] public Entity Host;
    [HideInInspector] public Entity lastHost;
    [HideInInspector] public Enemy Target;
    public virtual SpriteRenderer render => _render;


    public UnityAction OnAttacked;
    public UnityAction OnNotAttacked;

    protected Reward reward => GetComponent<Reward>();
    protected ShowName showname => GetComponent<ShowName>();
    public abstract Vector3 PositionStartAttack
    {
        get;
    }
    protected abstract bool ReadyToAttack
    {
        get;
    }
    public virtual float TakeTied => 0;
    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        render.sprite = Picture;
        render.sortingLayerName = "Effect";
        render.sortingOrder = 15;
        showname.Hide();
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
            reset();
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
            Notification.NoticeBelow(Languages.getString("DaTrangBi") + " <color=" + getColorNameByLevelWeapon(TypeOfWeapon) + ">" + nameOfWeapon + "</color>");
            OnEquip();
        }
        TrangThai = trangthai;
    }

    public virtual void OverLoadAttacked()
    {

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

    public virtual void reset()
    {
        lastHost = Host;
        Host = null;
        if (lastHost != null)
        {
            transform.position = lastHost.transform.position;
        }
        transform.rotation = Quaternion.identity;
        render.flipX = false;
        render.flipY = false;
        transform.parent = TransformInstanceOnLoad.getTransform();
        ChooseMinapulation.PlayerChoose.Remove(reward);
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

    protected virtual void OnDestroy()
    {
    }

    public string GetName()
    {
        return nameOfWeapon;
    }

    public Color GetColorName()
    {
        return getColorByLevelWeapon(TypeOfWeapon);
    }

    public SpriteRenderer GetRender()
    {
        return render;
    }

    protected virtual void OnValidate()
    {
        if (render != null)
        {
            render.sprite = Picture;
            render.sortingLayerName = "Effect";
            render.sortingOrder = 15;
        }
    }
}
