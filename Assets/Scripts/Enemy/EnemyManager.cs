using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private ShowHPEnemy ShowHPPrefab;
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
        if (damadata.Mediated)
        {
            show.StartUp(damadata.TextMediated);
        } else
        {
            show.StartUp(damadata.Damage, damadata.Type, damadata.IsCritical);
        }
    }

    public ShowHPEnemy ShowHP(Enemy enemy)
    {
        if (ShowHPPrefab == null)
        {
            Debug.Log("Ko có PrefabShowHP");
            return null;
        }
        ShowHPEnemy show = Instantiate(ShowHPPrefab, enemy.PR_HP);
        show.SetStart(enemy);
        return show;
    }
}
