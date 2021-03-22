using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    static string Path = "Data/AllRewards";
    static Rewards AllRewards;
    public static void LoadData()
    {
        AllRewards = Resources.Load<Rewards>(Path);
    }

    public static Reward GetRewardByName(string Name)
    {
        List <Reward> Rewards = new List<Reward>();
        // Thêm danh dách Reward
        Rewards.AddRange(AllRewards.rewardWeapons);
        Rewards.AddRange(AllRewards.rewardGolds);
        return Array.Find(Rewards.ToArray(), e => e.Name == Name);

    }

    public static Reward[] GetRewardsByName(string[] Name)
    {
        List<Reward> rewards = new List<Reward>();
        foreach (string name in Name)
        {
            Reward reward = GetRewardByName(name);
            if (reward != null)
            {
                rewards.Add(reward);
            }
        }
        return rewards.ToArray();
    }
}
