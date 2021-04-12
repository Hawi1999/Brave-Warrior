using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LegacyBuff : ScriptableObject, IGiveBuff, IShowBuffA, IBuffInGround
{
    [SerializeField] Sprite sprite;
    [SerializeField] string CodeThongBao;
    public virtual Sprite Sprite => sprite;
    protected virtual string ThongBao => Languages.getString(CodeThongBao);
    protected PlayerController host;
    public virtual void OnHostTake(Entity entity)
    {
        if (entity is PlayerController)
        {
            host = entity as PlayerController;
            if (host == PlayerController.PlayerCurrent)
            {
                Notification.NoticeBelow(ThongBao);
            }
        } else
        {

        }
    }
    protected virtual void OnTakeBuffValueChanged(int a)
    {

    }

    public LegacyBuff Clone()
    {
        LegacyBuff a = Instantiate(this);
        OnCreateClone(a);
        return a;
    }

    public void RemoveAllBuff()
    {
        if (host != null)
        {
            host.take.RemoveRegister(this);
        }
    }

    protected virtual void OnCreateClone(LegacyBuff clone)
    {

    }
}
