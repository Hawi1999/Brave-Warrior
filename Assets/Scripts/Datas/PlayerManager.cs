using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour

{
    public static int IDPlayer = 0;
    public List<PlayerController> PlayerPrefabs;
    public static PlayerManager Instance
    {
        get; set;
    }
    private void Awake()
    {
        Instance = this;
    }
}
