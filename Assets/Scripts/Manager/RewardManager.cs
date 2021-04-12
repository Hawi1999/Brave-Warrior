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

    public static List<Reward> GetRewards(TypeReward type)
    {
        List<Reward> Rewards = new List<Reward>();
        // Thêm danh dách Reward
        Rewards.AddRange(AllRewards.rewardWeapons);
        Rewards.AddRange(AllRewards.rewardGolds);
        List<Reward> re = new List<Reward>();
        foreach (Reward reward in Rewards)
        {
            if (reward.EqualTypeByChest(type))
            {
                Debug.Log("Add:" + reward.Name);
                re.Add(reward);
            }
        }
        return re;
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

    static string PathRewardBuff = "Prefabs/RewardBuff";
    public static void Buff2(Vector2 position, int level)
    {
        BuffInGround prefab = Resources.Load<BuffInGround>(PathRewardBuff);
        Buff2Data b = DataMap.GetBuff2(level);
        if (prefab == null || b == null)
        {
            Debug.Log("Khong tin tai Prefab hoac Buff");
            return;
        }
        BuffInGround buff = Instantiate(prefab);
        buff.transform.position = position;
        buff.SetUp(b);
    }

    /// <summary>
    /// Chua dinh nghia ham nay ma
    /// </summary>
    /// <param name="position"></param>
    /// <param name="level"></param>
    public static void LegacyBuff(Vector2 position)
    {
        BuffInGround prefab = Resources.Load<BuffInGround>(PathRewardBuff);
        LegacyBuff b = DataMap.GetLegacyBuff();
        if (prefab == null || b == null)
        {
            Debug.Log("Khong tin tai Prefab hoac Buff");
            return;
        }
        BuffInGround buff = Instantiate(prefab);
        buff.transform.position = position;
        buff.SetUp(b);
    }
}
