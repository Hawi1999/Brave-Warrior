using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class DrawMap : MonoBehaviour
{
    public Tilemap tilemapCR;
    public Tilemap tilemapN;
    public Tile Wall;
    public Tile N;
    public Vector2Int startPosition;
    public Vector2Int HsizeStart;
    public int HalfDistanceRound;
    public Vector2Int HsizeRound;
    [HideInInspector] public int numberRounds;
    public RoundData roundData;
    protected virtual void Start()
    {
        numberRounds = roundData.roundsData.Length;
        Draw();
    }
    protected virtual void Draw()
    {
        DrawN();
        DrawCR();
    }

    protected abstract void DrawCR();
    protected abstract void DrawN();

    protected abstract void SetRoundData();
}
