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

    private float lastChoose;
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
        if (Control.GetKey("X") && Time.time - lastChoose > 0.5f)
        {
            if (Choosing != null)
            {
                Choosing.TakeReward(player);
                lastChoose = Time.time;
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
            Control.OnWaitToClick?.Invoke("X");
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
        if (Rewards.Count == 0)
            Control.OnEndWaitToClick?.Invoke("X");
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
