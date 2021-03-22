using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RoundDatas", menuName = "Data/RoundDatas")]
public class RoundDatas : ScriptableObject
{
    public CodeMap codeMap = CodeMap.Map1;
    [SerializeField]
    RoundData[] roundsData;

    public RoundData GetRound(int a)
    {
        return roundsData[a];
    }

    public int NumberRound => roundsData.Length;
}
