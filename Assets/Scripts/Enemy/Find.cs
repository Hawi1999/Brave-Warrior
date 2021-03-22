using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Find 
{
    public static IFindTarget FindTargetNearest(Vector3 goc, float BanKinh, LayerMask layer)
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(goc, BanKinh, layer);
        List <IFindTarget> list = new List<IFindTarget>();
        if (cols == null && cols.Length == 0)
        {
            return null;
        }
        foreach (Collider2D col in cols)
        {
            IFindTarget target = col.GetComponent<IFindTarget>();
            if (target != null)
            {
                if (target.IsForFind)
                {
                    list.Add(target);
                }
            }
            
        }
        if (list.Count == 0)
        {
            return null;
        }
        float DistanceMin = Vector2.Distance(goc, list[0].center);
        int id = 0;
        for (int i = 1; i < list.Count; i++)
        {
            float Distance = Vector2.Distance(goc, list[i].center);
            if (Distance < DistanceMin)
            {
                DistanceMin = Distance;
                id = i;
            }

        }
        return list[id];
    }
}
