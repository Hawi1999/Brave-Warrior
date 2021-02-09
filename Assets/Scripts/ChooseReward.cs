using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChooseReward : MonoBehaviour
{
    public static ChooseReward Instance;
    [HideInInspector] public List<Reward> Rewards;
    private Reward rewardcr;
    private PlayerController player => PlayerController.PlayerCurrent;


    void Awake()
    {
        Instance = this;
    }
    public Reward Choosing
    {
        get
        {
            return rewardcr;
        }
        set
        {
            rewardcr = value;
            OnChooseReward?.Invoke(value);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (Choosing != null)
            {
                Choosing.TakeReward(player);

            }
        }
    }

    public void Add(Reward reward)
    {
        if (Rewards == null)
        {
            Rewards = new List<Reward>();
        }
        if (!isExist(reward))
        {
            Rewards.Add(reward);
            OnChooseReward += reward.Choose;
        }
        Choosing = reward;
    }

    public void Remove(Reward reward)
    {
        if (Rewards != null && isExist(reward))
        {
            Rewards.Remove(reward);
            if (Rewards.Count == 0)
            {
                Choosing = null;
            } else
            {
                Choosing = Rewards[Rewards.Count - 1];
            }
            OnChooseReward -= reward.Choose;
        }
    }

    private bool isExist(Reward reward)
    {
        foreach (Reward reward1 in Rewards)
        {
            if (reward == reward1)
            {
                return true;
            }
        }
        return false;
    }

    UnityAction<Reward> OnChooseReward;
}
