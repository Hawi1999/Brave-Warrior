using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RewardGold : Reward
{
    public override string Name => "Reward Gold " + Level.ToString();
    [Range(1, 3)]
    [SerializeField] int Level = 1;
    int amount;
    protected bool showed = false;
    
    public int Amount => amount;
    [SerializeField] TakeGoldFromMap Prejabs;

    public override bool WaitingForChoose => true;

    public override bool EqualTypeByChest(TypeReward type)
    {
        if (Level == 1 && type == TypeReward.Gold1)
        {
            return true;
        }
        if (Level == 2 && type == TypeReward.Gold2)
        {
            return true;
        }
        if (Level == 3 && type == TypeReward.Gold3)
        {
            return true;
        }
        return false;
    }

    public override void OnPlayerInto()
    {
        if (!showed)
        {
            amount = Random.Range(5 * Level, 10 * Level + 1);
            for (int i = 0; i < Amount; i++)
            {
                TakeGoldFromMap tg = Instantiate(Prejabs, transform.position, Quaternion.identity);
                tg.MoveToPosion(transform.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0));
            }
            showed = true;
        }
    }

    protected override void Update()
    {
        
    }
}
