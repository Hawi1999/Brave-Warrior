using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new LockRoomData", menuName = "Data/LockRoom")]
public class LockRoomDatas : ScriptableObject
{
    public CodeMap codeMap = CodeMap.Map1;
    public Direct direct;
    public LockRoom Preafabs;

    public enum Direct
    {
        Horizontal,
        Vertical
    }
}
