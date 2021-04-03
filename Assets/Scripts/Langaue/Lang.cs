using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Code", menuName = "Language/Strings")]
public class Lang : ScriptableObject
{
    public string CODE => name;
    [TextArea(1, 5)]
    public string VietNam = "text Vietnamese";
    [TextArea(1, 5)]
    public string English = "text English";
}
