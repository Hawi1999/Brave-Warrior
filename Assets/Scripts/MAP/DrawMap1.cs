using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawMap1 : DrawMap
{

    public Tile N1;
    public Tile N2;
    public LockRoom LR;

    private Vector3Int CenterCurrent;
    private int numberCurrent;
    protected override void Draw()
    {
        CenterCurrent = (Vector3Int)startPosition;
        DrawRoundStartN();
        DrawRoundStartCR();
        CenterCurrent.y += HsizeStart.y;
        for (int i = 0; i < numberRounds; i++)
        {
            numberCurrent = i;
            CenterCurrent.y += 1 + HalfDistanceRound;
            DrawDistance();
            CenterCurrent.y += HalfDistanceRound + 1 + HsizeRound.y;
            DrawN();
            DrawCR();
            SetRoundData();
            CenterCurrent.y += HsizeRound.y;
        }
    }

    private void DrawRoundStartCR()
    {
        for (int i = -HsizeStart.y - 1; i < HsizeStart.y + 1; i++)
        {
            for (int j = -HsizeStart.x - 1; j < HsizeStart.x + 1; j++)
            {
                if (i == -HsizeStart.y - 1 || i == HsizeStart.y || j == -HsizeStart.x - 1 || j == HsizeStart.x)
                {
                    if (i == HsizeStart.y && j >= - 2 && j <= + 1);
                    else
                    {
                        tilemapCR.SetTile(new Vector3Int(j, i, 0) + CenterCurrent, Wall);
                    }
                }
            }
        }
        Instantiate(LR, CenterCurrent + new Vector3(0, HsizeStart.y, 0), Quaternion.identity);
    }

    private void DrawRoundStartN()
    {
        for (int i = -HsizeStart.y; i < HsizeStart.y; i++)
        {
            for (int j = -HsizeStart.x; j < HsizeStart.x; j++)
            {
                tilemapN.SetTile(new Vector3Int(j, i, 0) + CenterCurrent, N1);
            }
        }
        for (int i = -2; i < 2; i++)
        {
            tilemapN.SetTile(new Vector3Int(i,HsizeStart.y, 0) + CenterCurrent, N2);
        }

    }
    private void DrawDistance()
    {
        // Ve tuong
        for (int i = -HalfDistanceRound; i < HalfDistanceRound; i++)
        {
            tilemapCR.SetTile(new Vector3Int(-3, i, 0) + CenterCurrent, Wall);
            tilemapCR.SetTile(new Vector3Int(2, i, 0) + CenterCurrent, Wall);
        }
        // Ve nen
        for (int i = -HalfDistanceRound; i < HalfDistanceRound; i++)
        {
            for (int j = -2; j < 2; j++)
            {
                tilemapN.SetTile(new Vector3Int(j, i, 0) + CenterCurrent, N1);
            }
        }
    }
    protected override void DrawCR()
    {
        for (int i = -HsizeRound.y - 1; i < HsizeRound.y + 1; i++)
        {
            for (int j = -HsizeRound.x - 1; j < HsizeRound.x + 1; j++)
            {
                if (i == -HsizeRound.y - 1 || i == HsizeRound.y || j == -HsizeRound.x - 1 || j == HsizeRound.x)
                {
                    if ((i == HsizeRound.y || i == - HsizeRound.y - 1)  && j >= -2 && j <= +1) ;
                    else
                    {
                        tilemapCR.SetTile(new Vector3Int(j, i, 0) + CenterCurrent, Wall);
                    }
                }
            }
        }
    }

    protected override void DrawN()
    {
        for (int i = -HsizeRound.y ; i < HsizeRound.y ; i++)
        {
            for (int j = -HsizeRound.x ; j < HsizeRound.x ; j++)
            {
                tilemapN.SetTile(new Vector3Int(j, i, 0) + CenterCurrent, N1);
            }
        }
        for (int i = -2; i < 2; i++)
        {
            tilemapN.SetTile(new Vector3Int(i, HsizeRound.y, 0) + CenterCurrent, N2);
            tilemapN.SetTile(new Vector3Int(i, -HsizeRound.y - 1, 0) + CenterCurrent, N2);
        }
    }

    protected override void SetRoundData()
    {
        RoundBase rb = Instantiate(roundData.Prefabs);
        rb.setUp(CenterCurrent, roundData.roundsData[numberCurrent].dots, tilemapCR, HsizeRound);
    }
}
