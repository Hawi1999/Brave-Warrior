using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Buff", menuName = "Data/Buff2Datas (Lưu dánh sách Buff2Data)")]
public class Buff2Datas : ScriptableObject
{
    [SerializeField] int level;
    public int Level => level;
    public List<int> UuTien = new List<int>();
    public List<Buff2Data> listBuffs = new List<Buff2Data>();
    public Buff2Datas Clone
    {
        get
        {
            return (Buff2Datas)MemberwiseClone();
        }
    }
    public Buff2Data GetBuff
    {
        get
        {
            Buff2Data b = GameController.GetRandomItem(ut: UuTien, a: listBuffs);
            if (b == null)
            {
                return null;
            }
            return b.Clone() as Buff2Data;
        }
    }
}
