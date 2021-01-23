using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RoundsData
{
    public List<Dot> dots;
}

[CreateAssetMenu(fileName = "New RoundData", menuName = "Data/RoundData")]
public class RoundData : ScriptableObject
{
    public RoundBase Prefabs;
    public RoundsData[] roundsData;
}
