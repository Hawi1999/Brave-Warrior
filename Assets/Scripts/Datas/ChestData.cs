using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chest", menuName = "Chest/new Chest")]
public class ChestData : ScriptableObject, IUpdateItemEditor
{
    public TypeChest Type = TypeChest.Start;
    public CodeMap codeMap = CodeMap.Map1;
    public int[] UuTien;
    public TypeReward[] NameOfRewards;



    public Reward getRandomReward()
    {
        int tong = 0;
        for (int i = 0; i < UuTien.Length; i++)
        {
            tong += UuTien[i];
        }
        int random = UnityEngine.Random.Range(1, tong + 1);
        TypeReward kq = TypeReward.WeaponCommon;
        for (int i = 0; i < NameOfRewards.Length; i++)
        {
            random -= UuTien[i];
            if (random <= 0)
            {
                kq = NameOfRewards[i];
                break;
            }
        }
        Debug.Log(kq.ToString());
        List<Reward> rewards = RewardManager.GetRewards(kq);
        if (rewards.Count == 0)
        {
            return null;
        }
        return rewards[UnityEngine.Random.Range(0, rewards.Count)];

    }
    private void OnValidate()
    {
        OnUpdate();
    }

    public void OnUpdate()
    {
        var s = Enum.GetValues(typeof(TypeReward));
        TypeReward[] n = s as TypeReward[];
        NameOfRewards = new TypeReward[s.Length];
        for (int i = 0; i < s.Length; i++)
        {
            NameOfRewards[i] = n[i];
        }

        int max = Mathf.Max(UuTien.Length, NameOfRewards.Length);
        int[] x = new int[NameOfRewards.Length];
        for (int i = 0; i < max; i++)
        {
            x[i] = UuTien[i];
        }
        UuTien = x.Clone() as int[];
    }
}
