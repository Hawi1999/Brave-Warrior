using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private ShowHPEnemy ShowHPPrefab;
    [SerializeField] private ShowHPSub ShowHPSubPrefab;
    [SerializeField] private VFXSpawn VFXSpawnPrefabs;



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

    public Enemy Spawn(Enemy Prefab, Vector3 position)
    {
        Enemy enemy = Instantiate(Prefab, position, Quaternion.identity);
        if (VFXSpawnPrefabs == null)
        {
            Debug.Log("Không có VFXSpawn, không thể spawn enemy");
            enemy.gameObject.SetActive(true);
        } else
        {
            VFXSpawn v = Instantiate(VFXSpawnPrefabs, position, Quaternion.identity);
            enemy.gameObject.SetActive(false);
            v.OnCompleteVFX += () => enemy.gameObject.SetActive(true);
        }
        return enemy;
    }

    public Enemy Spawn(Enemy Prefab, Vector3 position, Vector2[] limitMove)
    {
        Enemy enemy = Spawn(Prefab, position);
        enemy.setLimitMove(limitMove);
        return enemy;
    }

    public Enemy Spawn(Enemy Prefab, Vector3 position, Transform transform)
    {
        Enemy enemy = Spawn(Prefab, position);
        enemy.transform.parent = transform;
        return enemy;
    }

    public Enemy Spawn(Enemy Prefab, Vector3 position,Transform transform, Vector2[] limitMove)
    {
        Enemy enemy = Spawn(Prefab, position);
        enemy.setLimitMove(limitMove);
        enemy.transform.parent = transform;
        return enemy;
    }
}
