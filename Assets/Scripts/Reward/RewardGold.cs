﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardGold : Reward
{
    public override string Name => "Reward Gold " + Level.ToString();
    [Range(1, 3)]
    [SerializeField] int Level = 1;
    int amount;
    public int Amount => amount;
    [SerializeField] TakeGoldFromMap Prejabs;

    public override bool WaitingForGet => throw new System.NotImplementedException();

    public override void Appear()
    {
        amount = Random.Range(5 * Level, 10 * Level + 1);
        for (int i = 0; i < Amount; i++)
        {
            TakeGoldFromMap tg = Instantiate(Prejabs, transform.position, Quaternion.identity);
            tg.MoveToPosion(transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0));
        }
    }

    public override void Choose(Reward reward)
    {
        
    }

    public override void TakeReward(PlayerController host)
    {
        
    }
}
