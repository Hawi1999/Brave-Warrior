using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New List Rewards", menuName = "Data/List Rewards")]
public class Rewards : ScriptableObject, IUpdateItemEditor
{
    public List<RewardWeapon> rewardWeapons = new List<RewardWeapon>();
    public List<RewardGold> rewardGolds = new List<RewardGold>();


    [Header("Stick One Shot To Update List")]
    public bool Update = false;

    public void OnUpdate()
    {
        Debug.Log("Updated List Rewards");
        Reward[] rewards = Resources.LoadAll<Reward>("");
        this.rewardGolds.Clear();
        this.rewardWeapons.Clear();
        foreach (Reward reward in rewards)
        {
            if (reward is RewardGold)
            {
                rewardGolds.Add(reward as RewardGold);
            }
            if (reward is RewardWeapon)
            {
                rewardWeapons.Add(reward as RewardWeapon);
            }
        }
    }
}
