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
    public abstract void Appear();
}
