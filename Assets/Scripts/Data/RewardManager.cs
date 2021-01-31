using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    public List<RewardWeapon> WeaponRewards;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    public static Reward GetRewardByName(string Name)
    {
        if (Instance == null)
            return null;
        List <Reward> Rewards = new List<Reward>();
        // Thêm danh dách Reward
        Rewards.AddRange(Instance.WeaponRewards);
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
