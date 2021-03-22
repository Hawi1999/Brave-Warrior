using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Data/Enemy")]
public class EnemyDatas : ScriptableObject
{
    public GroupEnemy[] groups = new GroupEnemy[1];
    public TypeEnemy type = 0;
    public CodeMap codeMap = CodeMap.Map1;
    [SerializeField] private Enemy enemy;
    public int Level = 1;
    public int Priority = 10;

    public virtual Enemy GetEnemy => enemy;
    public bool inGroup(GroupEnemy gr)
    {
        if (groups == null)
        {
            return false;
        }
        foreach(GroupEnemy g in groups)
        {
            if (g == gr)
            {
                return true;
            }
        }
        return false;
    }


}
