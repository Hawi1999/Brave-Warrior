using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoundBoss : RoundBase
{
    List<Enemy> enemtsSpawned;

    protected override void Awake()
    {
        base.Awake();
        enemtsSpawned = new List<Enemy>();
    }

    protected override void OnPLayerOnInFirst()
    {
        base.OnPLayerOnInFirst();
        StartRound();
    }

    private void StartRound()
    {
        Enemy prefab = DataMap.GetEnemyDatasBossPrefab().GetEnemy;
        if (prefab == null)
        {
            Debug.Log("Null a"); 
            EndRound();
            return;
        }
        CloseAllDoor();
        Enemy enemy = EntityManager.Instance.SpawnEnemy(prefab, transform.position, transform);
        enemy.OnDeath += (E) => OnHasEnemyDie(E as Enemy);
        enemy.OnSpawnEnemyMore += HasMoreEnemy;

    }
    private void HasMoreEnemy(Enemy e)
    {
        enemtsSpawned.Add(e);
        e.OnDeath += (E) => OnHasEnemyDie(E as Enemy);
        e.OnSpawnEnemyMore += HasMoreEnemy;
    }

    private void OnHasEnemyDie(Enemy e)
    {
        enemtsSpawned.Remove(e);
        if (enemtsSpawned.Count <= 0)
        {
            EndRound();
        }
    }

    private void EndRound()
    {
        Debug.Log("Chưa có hiệu ứng khi hết trận nè");
        OpenAllDoor();
    }
}
