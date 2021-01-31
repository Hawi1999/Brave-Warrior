using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private ShowHP ShowHPPrefab;
    [SerializeField] private ShowHPSub ShowHPSubPrefab;
    public LayerMask WallAndBarrier;
    public static EnemyManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    public void ShowHPSub(DamageData damadata)
    {
        if (ShowHPSubPrefab == null)
        {
            Debug.Log("Ko có PrefabShowHPSub");
            return;
        }
        if (!(damadata.To is Enemy))
        {
            Debug.Log("Dữ liệu sát thương ko tìm thấy mục tiêu");
            return;
        }
        Enemy enemy = damadata.To as Enemy;
        ShowHPSub show = Instantiate(ShowHPSubPrefab, enemy.PR_HPsub.transform.position, Quaternion.identity);
        show.StartUp(damadata.getDamage() + enemy.LastDamage, damadata.Type);
    }

    public void ShowHP(Enemy enemy)
    {
        if (ShowHPPrefab == null)
        {
            Debug.Log("Ko có PrefabShowHP");
            return;
        }
        ShowHP show = Instantiate(ShowHPPrefab, enemy.PR_HP);
        show.SetStart(enemy);
    }
}
