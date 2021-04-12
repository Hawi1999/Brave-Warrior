using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeReward
{
    Gold1,
    Gold2,
    Gold3,
    WeaponCommon,
    WeaponEpic,
    WeaponRare,
    WeaponVeryRare,
    WeaponLegendary,
}
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Reward: MonoBehaviour, IManipulation
{
    [SerializeField] float sizeCollider = 3f;
    protected PlayerController player => PlayerController.PlayerCurrent;
    public abstract string Name
    {
        get;
    }
    protected bool going = false;
    protected bool choosing = false;
    public virtual bool WaitingForChoose
    {
        get
        {
            return going;
        }
    }
    public abstract bool EqualTypeByChest(TypeReward type);
    protected virtual void Awake()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (GetComponent<BoxCollider2D>() == null)
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
        }
        collider.size = new Vector2(3, 3);
        collider.isTrigger = true;
    }

    protected virtual void Update()
    {
        if (player != null)
        {
            if (WaitingForChoose)
            {
                ChooseMinapulation.PlayerChoose.Add(this);
            }
            else if (player != null)
            {
                ChooseMinapulation.PlayerChoose.Remove(this);
            }
        }
    }

    /// <summary>
    /// Ham nay duoc goi khi doi tuong chon duoc thao tac X
    /// </summary>
    /// <param name="host"></param>
    public virtual void TakeManipulation(PlayerController host)
    {
        host.PLayerChoose.Remove(this);
    }
    /// <summary>
    /// Ham nay duoc goi khi doi tuong chon bi thay doi
    /// </summary>
    /// <param name="manipulation"> Doi tuong dang chon </param>
    public virtual void OnChoose(IManipulation manipulation)
    {
        if (manipulation != null || manipulation as Object != null)
        {
            choosing = (manipulation as Object == this);
        } else
        {
            choosing = false;
        }
    }
    /// <summary>
    /// ham nay duoc goi khi PLayer den gan 
    /// </summary>
    public virtual void OnPlayerInto()
    {

    }
    /// <summary>
    /// ham nay duoc goi khi PLayer di ra xa
    /// </summary>
    public virtual void OnPlayerOutTo()
    {

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!going)
            {
                OnPlayerInto();
                going = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (going)
            {
                OnPlayerInto();
                going = false;
            }
        }
    }
}
