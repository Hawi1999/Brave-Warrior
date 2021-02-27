using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManipulation 
{
    bool WaitingForChoose { get; }
    void TakeManipulation(PlayerController host);
    void OnChoose(IManipulation reward);
}
