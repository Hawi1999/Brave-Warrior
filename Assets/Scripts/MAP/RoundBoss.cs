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
        PlayerController.PlayerCurrent.setLimitMove(Data.GetPositionLimit());
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
        List<Vector2> listposspawn = new List<Vector2>();
        for (int i = 0; i < Data.AmountBoss; i++)
        {
            listposspawn.Add(Vector2.zero);
        }
        switch (Data.AmountBoss)
        {
            case 1:
                listposspawn[0] = Data.position;
                break;
            case 2:
                listposspawn[0] = Data.position + new Vector2(-Data.Size.x / 4, 0);
                listposspawn[1] = Data.position + new Vector2(Data.Size.x / 4, 0);
                break;
            case 3:
                listposspawn[0] = Data.position + new Vector2(-Data.Size.x / 4, 0);
                listposspawn[1] = Data.position + new Vector2(Data.Size.x / 4, 0);
                listposspawn[2] = Data.position + new Vector2(0,Data.Size.y / 4);
                break;
            case 4:
                listposspawn[2] = Data.position + new Vector2(0, Data.Size.y / 4);
                listposspawn[0] = Data.position + new Vector2(-Data.Size.x / 4, 0);
                listposspawn[1] = Data.position + new Vector2(Data.Size.x / 4, 0);
                listposspawn[3] = Data.position + new Vector2(0, -Data.Size.y / 4);
                break;


        }
        for (int i = 0; i < Data.AmountBoss; i++)
        {
            Enemy enemy = EntityManager.Instance.SpawnEnemy(prefab, listposspawn[i], transform);
            HasMoreEnemy(enemy);
        }

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
        OpenAllDoor();
        RoundComplete();
        RoundCurrent = null;
        PlayerController.PlayerCurrent.setLimitMove(null);
    }

    protected override void RoundComplete()
    {
        base.RoundComplete();
        RewardManager.LegacyBuff(TileManager.GetPositionInGoundCurrent());
    }
}
