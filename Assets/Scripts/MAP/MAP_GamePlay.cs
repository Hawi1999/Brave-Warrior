using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(DrawMap))]
public class MAP_GamePlay : MAPController
{
    [SerializeField]
    CodeMap codeMap;
    [SerializeField]
    GroupEnemy codeEnemy;
    RoundDatas rounds;
    public static CodeMap CodeMapcurent;
    public static GroupEnemy CodeEnemyCurrent;

    static string PathCanvasMain = "Prefabs/CanvasGamePlay";
    protected override void Awake()
    {
        base.Awake();
        CodeMapcurent = codeMap;
        CodeEnemyCurrent = codeEnemy;
    }
    protected override void Start()
    {
        base.Start();
        SetUpRounds();
        PlayerController.PlayerCurrent.gameObject.SetActive(false);
        PlayerController.PlayerCurrent.OnDeath += (a) => WhenPlayerDied();
        CanvasMain cvm = Resources.Load<CanvasMain>(PathCanvasMain);
        if (cvm != null)
        {
            Instantiate(cvm);
        }
    }

    private void WhenPlayerDied()
    {
        Invoke("ShownChooseAfterDie", 2f);
    }

    private void ShownChooseAfterDie()
    {
        ShowTableDied s = Resources.Load<ShowTableDied>("Prefabs/CANVASdie");
        if (s != null)
        {
            Instantiate(s, GameController.CanvasMain.transform);
        } else
        {
            GameController.Instance.LoadScene("TrangTrai");
        }
    }

    private void SetUpRounds()
    {
        // SetUpInfo
        Vector2Int positionCurrent = PositionPlayerStart;
        rounds = DataMap.GetRoundDatas();
        Direct lasDir = Direct.Down;
        for (int i = 0; i < rounds.NumberRound - 1; i++)
        {
            RoundData roundCurrent = rounds.GetRound(i);
            roundCurrent.position = positionCurrent;
            List<Direct> drs = new List<Direct>();
            drs.Add(Direct.Left);
            drs.Add(Direct.Right);
            drs.Add(Direct.Up);
            if (lasDir == Direct.Right)
            {
                roundCurrent.AddDirect(Direct.Left);
                drs.Remove(Direct.Left);
            }
            if (lasDir == Direct.Left)
            {
                roundCurrent.AddDirect(Direct.Right);
                drs.Remove(Direct.Right);
            }
            if (lasDir == Direct.Up)
            {
                roundCurrent.AddDirect(Direct.Down);
            }
            int a = Random.Range(0, drs.Count);
            lasDir = drs[a];
            roundCurrent.AddDirect(lasDir);
            positionCurrent = GetNewPosition(positionCurrent, lasDir);
        }
        RoundData bossRound = rounds.GetRound(rounds.NumberRound - 1);
        bossRound.position = positionCurrent;
        if (lasDir == Direct.Right)
        {
            bossRound.AddDirect(Direct.Left);
        }
        if (lasDir == Direct.Left)
        {
            bossRound.AddDirect(Direct.Right);
        }
        if (lasDir == Direct.Up)
        {
            bossRound.AddDirect(Direct.Down);
        }
        for (int i = 0; i < rounds.NumberRound; i++)
        {
            RoundData r = rounds.GetRound(i);
            SetRoundData(r);
            DrawMap.Draw(r.textMap, (Vector3Int)r.position);
        }
        for (int i = 0; i < rounds.NumberRound - 1; i++)
        {
            RoundData r1 = rounds.GetRound(i);
            RoundData r2 = rounds.GetRound(i + 1);
            Vector2Int p1 = r1.position;
            Vector2Int p2 = r2.position;
            if (p1.x == p2.x)
            {
                DrawMap.DrawConnect((Vector3Int)r1.GetPositionOutSide(Direct.Up), (Vector3Int)r2.GetPositionOutSide(Direct.Down));
            } else
            if (p1.y == p2.y)
            {
                if (p1.x < p2.x)
                {
                    DrawMap.DrawConnect((Vector3Int)r1.GetPositionOutSide(Direct.Right), (Vector3Int)r2.GetPositionOutSide(Direct.Left));
                } else
                {
                    DrawMap.DrawConnect((Vector3Int)r2.GetPositionOutSide(Direct.Right), (Vector3Int)r1.GetPositionOutSide(Direct.Left));
                }
            }
        }
    }

    private Vector2Int GetNewPosition(Vector2Int current, Direct huong)
    {
        if (huong == Direct.Right)
        {
            return current + 35 * Vector2Int.right;
        }
        if (huong == Direct.Left)
        {
            return current + 35 * Vector2Int.left; 
        }
        if (huong == Direct.Up)
        {
            return current + 25 * Vector2Int.up;
        }
        return current;
    }

    private void RoundFinalComplete(RoundBase r)
    {
        if (r.Data == rounds.GetRound(rounds.NumberRound - 1))
        {
            Invoke("ComingHome", 3f);
        }
    }

    private void ComingHome()
    {
        GameController.Instance.LoadScene("TrangTrai");
    }

    private RoundBase SetRoundData(RoundData round)
    {
        RoundBase r = null;
        round.textMap = (DataMap.GetSpriteMap(codeMap, round.typeRound));
        switch (round.typeRound)
        {
            case TypeRound.Enemy:
                r = new GameObject("Round Enemy")
                    .AddComponent<RoundEnemy>();
                break;
            case TypeRound.Chest:
                r = new GameObject("Round Chest")
                    .AddComponent<RoundChest>();
                break;
            case TypeRound.Boss:
                r = new GameObject("Round Boss")
                    .AddComponent<RoundBoss>();
                break;
            case TypeRound.Begin:
                r = new GameObject("Round Begin")
                    .AddComponent<RoundBegin>();
                break;
            case TypeRound.Hail:
                r =new GameObject("Round Hail")
                    .AddComponent<RoundHail>();
                break;
        }
        r.transform.parent = transform;
        r.SetUp(round);
        r.SetLocKRoom(round.GetDirects().ToArray());
        r.OnRoundComplete += RoundFinalComplete;
        return r;
    }

    public override void SetPlayerPositionInMap()
    {
        SetPlayerPosDefault();
    }

    protected override PlayerController CreatePlayer()
    {
        return null;
    }

    public static void StartGame()
    {
        var updateLanguages = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IBattle>();
        foreach (IBattle i in updateLanguages)
        {
            i.OnSceneStarted();
        }
    }

    protected override void StartAudio()
    {
        SoundManager.PlayBackGround(DataMap.GetClip(ClipDatas.Type.RoundNormal), volume: 0);
        StartCoroutine(SoundManager.ChangeValueBackGround(0.3f, 0.5f));
    }
}
