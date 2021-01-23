using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerController))]
public class ChooseReward : MonoBehaviour
{
    [HideInInspector] public List<Reward> Rewards;
    private Reward rewardcr;
    private PlayerController player;
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

    private void Start()
    {
        player = GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (Choosing != null)
            {
                Choosing.TakeReward(player);
                OnChooseReward -= Choosing.Choose;
            }
        }
    }

    public void Add(Reward reward)
    {
        if (Rewards == null)
        {
            Rewards = new List<Reward>();
        }
        if (isNotExist(reward))
        {
            Rewards.Add(reward);
            OnChooseReward += reward.Choose;
        }
        Choosing = reward;
    }

    public void Remove(Reward reward)
    {
        if (Rewards != null)
        {
            Rewards.Remove(reward);
        }
        if (Rewards.Count == 0)
        {
            Choosing = null;
        } else
        {
            Choosing = Rewards[Rewards.Count - 1];
        }
        OnChooseReward -= reward.Choose;

    }

    private bool isNotExist(Reward reward)
    {
        foreach (Reward reward1 in Rewards)
        {
            if (reward == reward1)
            {
                return false;
            }
        }
        return true;
    }

    UnityAction<Reward> OnChooseReward;
}
