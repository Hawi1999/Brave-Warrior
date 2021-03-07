using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindEnemy 
{
    public static Enemy FindEnemyNearest(Vector3 goc, float BanKinh, LayerMask layer)
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(goc, BanKinh, layer);
        List <Enemy> list = new List<Enemy>();
        if (cols == null && cols.Length == 0)
        {
            return null;
        }
        foreach (Collider2D col in cols)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemy.IsForFind)
                    list.Add(enemy);
            }
            
        }
        if (list.Count == 0)
        {
            return null;
        }
        float DistanceMin = Vector2.Distance(goc, list[0].PositionColliderTakeDamage);
        int id = 0;
        for (int i = 1; i < list.Count; i++)
        {
            float Distance = Vector2.Distance(goc, list[i].PositionColliderTakeDamage);
            if (Distance < DistanceMin)
            {
                DistanceMin = Distance;
                id = i;
            }

        }
        return list[id];
    }
}
