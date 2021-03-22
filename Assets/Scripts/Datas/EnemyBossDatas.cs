using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New EnemysData", menuName = "Data/Enemys")]
public class EnemyBossDatas : EnemyDatas
{
    [SerializeField]
    Enemy[] enemysBoss;

    bool[] x;

    public override Enemy GetEnemy
    {
        get
        {
            int a = 0;
            foreach (bool x in x)
            {
                if(!x)
                {
                    a++;
                }
            }
            if (a == 0)
                return null;
            int b = Random.Range(0, a);
            for (int i = 0; i < x.Length; i++)
            {
                if (!x[i])
                {
                    if (b == 0)
                    {
                        return enemysBoss[i];
                    } else
                    {
                        b--;
                    }
                }
            }
            return null;
        }
    }

    private void OnEnable()
    {
        if (enemysBoss != null)
        {
            x = new bool[enemysBoss.Length];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = false;
            }
        }
    }

}
