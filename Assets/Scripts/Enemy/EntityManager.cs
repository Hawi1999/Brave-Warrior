using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] private ShowHPEnemy ShowHPPrefab;
    [SerializeField] private ShowText ShowHPSubPrefab;
    [SerializeField] private VFXSpawn VFXSpawnPrefabs;



    public LayerMask WallAndBarrier;
    public static EntityManager Instance
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
        List<string> list = damadata.GetStringShow();
        foreach (string s in list)
        {
            ShowText show = Instantiate(ShowHPSubPrefab, enemy.PR_HPsub.transform.position, Quaternion.identity);
            show.SetText(s);
        }
    }

    public ShowHPEnemy ShowHP(Enemy enemy)
    {
        if (ShowHPPrefab == null)
        {
            Debug.Log("Ko có PrefabShowHP");
            return null;
        }
        ShowHPEnemy show = Instantiate(ShowHPPrefab, enemy.PR_HP.position, Quaternion.identity);
        show.SetStart(enemy);
        return show;
    }

    public Enemy SpawnEnemy(Enemy Prefab, Vector3 position)
    {
        Enemy enemy = Instantiate(Prefab, position, Quaternion.identity);
        if (VFXSpawnPrefabs == null)
        {
            Debug.Log("Không có VFXSpawn, không thể spawn enemy");
            enemy.gameObject.SetActive(true);
        } else
        {
            VFXSpawn v = Instantiate(VFXSpawnPrefabs, position, Quaternion.identity);
            enemy.Spawning();
            v.OnCompleteVFX += enemy.BeginInRound;
        }
        return enemy;
    }

    public Enemy SpawnEnemy(Enemy Prefab, Vector3 position, Vector2[] limitMove)
    {
        Enemy enemy = SpawnEnemy(Prefab, position);
        enemy.setLimitMove(limitMove);
        return enemy;
    }

    public Enemy SpawnEnemy(Enemy Prefab, Vector3 position, Transform transform)
    {
        Enemy enemy = SpawnEnemy(Prefab, position);
        enemy.transform.parent = transform;
        return enemy;
    }

    public Enemy SpawnEnemy(Enemy Prefab, Vector3 position,Transform transform, Vector2[] limitMove)
    {
        Enemy enemy = SpawnEnemy(Prefab, position);
        enemy.setLimitMove(limitMove);
        enemy.transform.parent = transform;
        return enemy;
    }

    public void SpawnPlayer()
    {
        PlayerController player = PlayerController.PlayerCurrent;
        if (player == null)
            return;
        VFXSpawn v = Instantiate(VFXSpawnPrefabs, player.transform.position, Quaternion.identity);
        player.Spawning();
        v.OnCompleteVFX += player.BeginInRound;
    }

    public static Color GetColorByElement(DamageElement ele)
    {
        if (ele == DamageElement.Fire)
        {
            return Color.red;
        }
        if (ele == DamageElement.Ice)
        {
            return Color.blue;
        }
        if (ele == DamageElement.Poison)
        {
            return Color.green;
        }
        if (ele == DamageElement.Electric)
        {
            return Color.yellow;
        }
        return Color.white;
    }

    public static string GetNameOfColorByElement(DamageElement ele)
    {
        if (ele == DamageElement.Fire)
        {
            return "red";
        }
        if (ele == DamageElement.Ice)
        {
            return "blue";
        }
        if (ele == DamageElement.Poison)
        {
            return "green";
        }
        if (ele == DamageElement.Electric)
        {
            return "yellow";
        }
        return "white";
    }
}
