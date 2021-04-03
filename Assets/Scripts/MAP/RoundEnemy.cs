using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoundEnemy : RoundBase
{
    protected int Current_IDDot;
    private GameObject PRSpawnEnemy;
    private List<LockRoom> LockRooms;
    private List<Enemy> ListEnemySpawned;
    private int idmax = 0;

    private void StartRound(int a)
    {
        if (a == 0)
        {
            CloseAllDoor();
        }
        SpawnEnemy(a);
        OnStartRound(a);
    }
    private void EndRound(int a)
    {
        OnEndRound(a);
        NextRound(a + 1);
    }
    private void HasEnemyDying(Entity entity)
    {
        Enemy enemy;
        if (entity is Enemy)
        {
            enemy = entity as Enemy;
        } else
        {
            return;
        }
        ListEnemySpawned.Remove(enemy);
        OnHasEnemyDie(enemy);
        if (ListEnemySpawned.Count == 0)
        {
           EndRound(Current_IDDot);
        }
    }
    private void NextRound(int id_new)
    {
        if (id_new > idmax)
        {
            RoundComplete();
            Clear();
        } else
        {
            Current_IDDot = id_new;
            StartRound(Current_IDDot);
        }
    }
    private void Clear()
    {
        OpenAllDoor();
        OnClear();
        RoundCurrent = null;
        PlayerController.PlayerCurrent.setLimitMove(null);
    }
    private void SpawnEnemy(int a)
    {

        if (PRSpawnEnemy == null)
        {
            PRSpawnEnemy = Instantiate(new GameObject("PR Enemy Spawned"), transform);
        }
        if (Data.Waves == null)
        {
            Clear();
            Debug.Log("Waves null?");
            return;
        }
        Wave v = Data.Waves.GetWave(a);
        List<Enemy> listEnemy = DataMap.GetListEnemyPrefab(v.TotalLevel, v.MinLevel, v.MaxLevel);
        if (listEnemy == null || listEnemy.Count == 0)
        {
            Debug.Log("Không có danh sánh Enemy trong đợt " + a + ", tiếp tục đợt tiếp theo.");
            NextRound(a + 1);
            return;
        }
        if (ListEnemySpawned == null)
        {
            ListEnemySpawned = new List<Enemy>();
        }
        foreach (Enemy LE in listEnemy)
        {

            Vector3 position = TileManager.GetPositionInGoundCurrent();
            Enemy ene = EntityManager.Instance.SpawnEnemy(LE, position, PRSpawnEnemy.transform, Data.GetPositionLimit());
            ene.OnDeath += HasEnemyDying;
            ListEnemySpawned.Add(ene);
        }
    }

    protected virtual void OnStartRound(int id)
    {
        if (id == 0)
        {
            PlayerController.PlayerCurrent.setLimitMove(Data.GetPositionLimit());
        }
    }

    protected virtual void OnClear()
    {
        Vector3 pos = TileManager.GetPositionInGoundCurrent();
        pos = new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), 0) + new Vector3(0.5f, 0.5f, 0);
        ChestManager.SpawnReWardChest(Data.colorChest, Data.typeChest, pos);
        PlayerController.PlayerCurrent.setLimitMove(null);
    }

    protected virtual void OnAfterClear()
    {

    }

    protected virtual void OnEndRound(int a)
    {
    }

    protected virtual void OnHasEnemyDie(Enemy enemy)
    {

    }

    protected override void OnPLayerOnInFirst()
    {
        base.OnPLayerOnInFirst();
        NextRound(0);
    }

    public override void SetUp(RoundData roundData)
    {
        base.SetUp(roundData);
        idmax = Data.Waves.NumberWave - 1;
    }


}
