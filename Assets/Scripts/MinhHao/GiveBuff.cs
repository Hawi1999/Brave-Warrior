using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GiveBuff : ScriptableObject
{
    [System.Serializable]
    public class BuffRegisterData
    {
        public BuffRegister.TypeBuff typeBuff = BuffRegister.TypeBuff.None;
        public float value;
    }
    public string codeThongBao = "";
    public Sprite sprite;
    [SerializeField] List<BuffRegisterData> buffs = new List<BuffRegisterData>();
    [HideInInspector]
    public int CloneID = 0;
    protected static int AmountID = 0;
    public GiveBuff Clone()
    {
        GiveBuff a = (GiveBuff)MemberwiseClone();
        a.CloneID = ++AmountID;
        return a;
    }
    private List<TakeBuff> takes = new List<TakeBuff>();
    public virtual void Register(TakeBuff a)
    {
        Notification.NoticeBelow(Languages.getString(codeThongBao));
        for (int i = 0; i < buffs.Count; i++) {
            a.Register(this, buffs[i].typeBuff, buffs[i].value);
            if (!takes.Contains(a))
            {
                takes.Add(a);
            }
        }
    }
    public void RemoveAll()
    {
        foreach (TakeBuff a in takes)
        {
            a.RemoveRegister(this);
        }
    }
    public override bool Equals(object other)
    {
        Buff2Data target = (Buff2Data)(other);
        if (target == null)
        {
            return false;
        }
        return (CloneID == target.CloneID);
    }
    public override int GetHashCode()
    {
        return CloneID.GetHashCode();
    }
}
