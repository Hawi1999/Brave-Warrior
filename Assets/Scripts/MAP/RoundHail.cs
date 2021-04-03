using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WaveHail
{
    [SerializeField] float speed = 3;
    [SerializeField] float time = 5;
    [SerializeField] float density = 0.3f;
    [SerializeField] Vector2Int rangeHP;

    public float Speed => speed;
    public float Time => time;
    public float Density => density;
    public Vector2Int RangeHp => rangeHP;
}

public class RoundHail : RoundBase
{
    private string Path = "Prefabs/Meteorite";
    private string PathSpawnZone = "Prefabs/MeteoriteSpawnZone";

    private int idmax;
    private int idcurrent;
    private static Meteorite Prefab;

    private static int idMeteo;
    private PoolingGameObject pool => PoolingGameObject.PoolingMain;
    private SpriteRenderer zoneSpawn;
    List<Meteorite> meteos = new List<Meteorite>();
    protected override void OnPLayerOnInFirst()
    {
        base.OnPLayerOnInFirst();
        CloseAllDoor();
        BeginRound();
    }

    protected override void Awake()
    {
        base.Awake();
        if (Prefab == null)
        {
            Prefab = Resources.Load<Meteorite>(Path);
            if (Prefab != null)
            {
                idMeteo = pool.AddPrefab(Prefab);
            } else
            {
                Debug.Log("Không tìm thấy Thiện thạch");
            }
        }
    }

    private void BeginRound()
    {
        if (Prefab == null || idmax == 0)
        {
            EndRound();
            return;
        }
        idcurrent = 0;
        NextRound();
    }

    public override void SetUp(RoundData roundData)
    {
        base.SetUp(roundData);
        idmax = Data.WavesHail.Length;
    }

    private void NextRound()
    {
        Vector2 Direction;
        Direct a;
        Direct b;
        Vector2 Node1;
        Vector2 Node2;
        int i = Random.Range(0, 4);
        bool isVer;
        float zZone;
        Vector2 posZone;
        switch (i)
        {
            case 0:
                Direction = Vector2.right;
                a = Direct.LeftUp;
                b = Direct.LeftDown;
                isVer = false;
                zZone = 270f;
                posZone = Data.GetPositionInSide(Direct.Left);
                break;
            case 1:
                Direction = Vector2.up;
                a = Direct.LeftDown;
                b = Direct.RightDown;
                isVer = true;
                zZone = 0;
                posZone = Data.GetPositionInSide(Direct.Down);
                break;
            case 2:
                Direction = Vector2.left;
                isVer = false;
                a = Direct.RightDown;
                b = Direct.RightUp;
                zZone = 90;
                posZone = Data.GetPositionInSide(Direct.Right);
                break;
            case 3:
                Direction = Vector2.down;
                a = Direct.RightUp;
                b = Direct.LeftUp;
                isVer = true;
                zZone = 180f;
                posZone = Data.GetPositionInSide(Direct.Up);
                break;
            default:
                Direction = Vector2.right;
                a = Direct.LeftUp;
                b = Direct.LeftDown;
                isVer = false;
                zZone = 270f;
                posZone = Data.GetPositionInSide(Direct.Left);
                break;
        }
        Node1 = Data.GetMidPositionInCellLimit(a);
        Node2 = Data.GetMidPositionInCellLimit(b);
        if (zoneSpawn == null)
        {
            SpriteRenderer x = Resources.Load<SpriteRenderer>(PathSpawnZone);
            if (x != null)
            {
                zoneSpawn = Instantiate(x);
            }
        }
        else
        {
            zoneSpawn.gameObject.SetActive(true);
        }
        if (zoneSpawn != null)
        {
            zoneSpawn.transform.position = posZone;
            zoneSpawn.transform.rotation = Quaternion.Euler(new Vector3(0, 0, zZone));
            zoneSpawn.transform.localScale = new Vector3((isVer ? Data.Size.x : Data.Size.y), 1, 1);
        }
        StartCoroutine(SpawnMeteorites(Node1, Node2, Direction, isVer, posZone));
    }

    private bool spawning = false;
    IEnumerator SpawnMeteorites(Vector2 Node1, Vector2 Node2, Vector2 Direction, bool isVer, Vector3 posZone)
    {
        yield return new WaitForSeconds(3f);
        CameraMove.Instance.AddPosition(new TaretVector3("Round Hail", ((Vector2)posZone + Data.position)/2));
        PlayerController.PlayerCurrent.DirectFire = -Direction;
        PlayerController.PlayerCurrent.LocKDirectFire = true;
        WaveHail waveCurrent = Data.WavesHail[idcurrent];
        float timeSpawned = 0;
        float timeWait = 0;
        float AmountPerSecond = waveCurrent.Density * (isVer ? Data.Size.x : Data.Size.y);
        float timeDelayPerMeteorite = 1 / AmountPerSecond;
        if (timeDelayPerMeteorite == 0)
        {
            Debug.Log("Timedelay = 0 nen fix lai 0.1f");
            timeDelayPerMeteorite = 0.1f;
        }
        spawning = true;
        while (timeSpawned < waveCurrent.Time)
        {
            yield return null;
            timeSpawned += Time.deltaTime;
            timeWait += Time.deltaTime;
            while (timeWait > timeDelayPerMeteorite)
            {
                Vector2 pos = GetRandomPositionInLine(Node1, Node2);
                Meteorite a = pool.Spawn(idMeteo, pos, MathQ.DirectionToQuaternion(Direction)) as Meteorite;
                a.StartUp(waveCurrent.Speed, Direction, Random.Range(waveCurrent.RangeHp.x, waveCurrent.RangeHp.y));
                meteos.Add(a);
                a.OnDead += CheckAmountMeteorite;
                timeWait -= timeDelayPerMeteorite;
            }
        }
        if (zoneSpawn != null)
        {
            zoneSpawn.gameObject.SetActive(false);
        }
        spawning = false;
    }
    private void CheckAmountMeteorite(Meteorite e)
    {
        meteos.Remove(e);
        e.OnDead -= CheckAmountMeteorite;
        if (meteos.Count == 0 && !spawning)
        {
            PlayerController.PlayerCurrent.LocKDirectFire = false;
            EndRound();
        }
    }

    private Vector2 GetRandomPositionInLine(Vector2 a, Vector2 b)
    {
        float x = (b.x - a.x) * Random.Range(0, 1f) + a.x;
        float y = (b.y - a.y) * Random.Range(0, 1f) + a.y;
        return new Vector2(x, y);

    }


    private void EndRound()
    {
        CameraMove.Instance.RemovePosition("Round Hail");
        if (idcurrent >= idmax - 1)
        {
            RoundComplete();
            Clear();
        } else
        {
            idcurrent++;
            NextRound();
        }
    }

    protected override void RoundComplete()
    {
        base.RoundComplete();
        RewardManager.Buff2(TileManager.GetPositionInGoundCurrent(), Data.Level);
    }

    private void Clear()
    {
        OpenAllDoor();
        RoundCurrent = null;
    }

    private void OnDestroy()
    {
        if (pool != null)
       {
            pool.RemovePrefab(idMeteo);
            idMeteo = 0;
        }
    }

}
