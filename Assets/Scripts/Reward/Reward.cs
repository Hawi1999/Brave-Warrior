using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward: MonoBehaviour, IManipulation
{

    protected PlayerController player => PlayerController.PlayerCurrent;
    public abstract string Name
    {
        get;
    }
    public abstract bool WaitingForChoose { get;}
    public abstract void TakeManipulation(PlayerController host);
    public abstract void OnChoose(IManipulation manipulation);
    public abstract void Appear();
}
