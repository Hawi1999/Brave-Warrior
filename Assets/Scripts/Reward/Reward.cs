using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Reward: MonoBehaviour, IManipulation
{
    [SerializeField] float sizeCollider = 1f;
    protected PlayerController player => PlayerController.PlayerCurrent;
    public abstract string Name
    {
        get;
    }
    protected bool going = false;
    protected bool taked = false;
    public virtual bool WaitingForChoose
    {
        get
        {
            return going && !taked;
        }
    }

    protected virtual void Awake()
    {
        if (GetComponent<BoxCollider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>().size = new Vector2(3, 3);
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
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
        taked = true;
        host.PLayerChoose.Remove(this);
    }
    /// <summary>
    /// Ham nay duoc goi khi doi tuong chon bi thay doi
    /// </summary>
    /// <param name="manipulation"> Doi tuong dang chon </param>
    public virtual void OnChoose(IManipulation manipulation)
    {

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
