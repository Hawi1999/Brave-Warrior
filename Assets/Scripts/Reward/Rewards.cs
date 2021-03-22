using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New List Rewards", menuName = "Data/List Rewards")]
public class Rewards : ScriptableObject 
{
    public List<RewardWeapon> rewardWeapons = new List<RewardWeapon>();
    public List<RewardGold> rewardGolds = new List<RewardGold>();
}
