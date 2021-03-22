using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Wave
{
    public int MinLevel;
    public int MaxLevel;
    public int TotalLevel;
}

[CreateAssetMenu(fileName = "W1", menuName = "Waves/New Waves")]
public class Waves : ScriptableObject
{
    [SerializeField] List<Wave> waves = new List<Wave>();

    public Wave GetWave(int a)
    {
        return waves[a];
    }

    public int NumberWave => waves.Count;
}
