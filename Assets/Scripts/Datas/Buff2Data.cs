using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Buff", menuName = "Data/Buff (Các buff tăng sức mạnh cho player")]
public class Buff2Data : ScriptableObject, IGiveBuff, IShowBuffA, IBuffInGround
{
    [System.Serializable]
    public class BuffRegisterData
    {
        public int typeBuff = BuffRegister.TypeBuff.None;
        public float value;
        [TextArea(2, 2)]
        public string codeName = "None";
    }
    public string codeThongBao = "";
    public Sprite sprite;
    [SerializeField] List<BuffRegisterData> buffs = new List<BuffRegisterData>();
    public Buff2Data Clone()
    {
        Buff2Data a = Instantiate(this);
        OnClone(a);
        return a;
    }

    protected virtual void OnClone(Buff2Data clone)
    {

    }
    TakeBuff host;
    public virtual void Register(TakeBuff a)
    {
        Notification.NoticeBelow(Languages.getString(codeThongBao));
        host = a;
        for (int i = 0; i < buffs.Count; i++)
        {
            a.Register(this, buffs[i].typeBuff, buffs[i].value);
        }
    }
    public void RemoveAllBuff()
    {
        if (host != null)
        {
            host.RemoveRegister(this);
        }
    }
    public Sprite Sprite => sprite;

    public void OnHostTake(Entity entity)
    {
        Register(entity.take);
    }

    private void OnValidate()
    {
        if (buffs == null || buffs.Count == 0)
        {
            return;
        }
        foreach (BuffRegisterData b in buffs)
        {
            b.codeName = BuffRegister.TypeBuff.GetStringCode(b.typeBuff);
        }
    }
}
