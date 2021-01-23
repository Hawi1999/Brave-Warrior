using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward: MonoBehaviour
{
    public abstract string Name
    {
        get;
    }
    public abstract bool WaitingForGet { get;}
    public abstract void TakeReward(PlayerController host);
    public abstract void Choose(Reward reward);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && WaitingForGet)
        {
            ChooseReward chooseReward = collision.GetComponent<ChooseReward>();
            chooseReward.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && WaitingForGet)
        {
            ChooseReward chooseReward = collision.GetComponent<ChooseReward>();
            chooseReward.Remove(this);
        }
    }
}
