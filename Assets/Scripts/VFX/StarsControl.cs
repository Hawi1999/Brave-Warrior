using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class StarsControl : MonoBehaviour
{
    public Bui bui;
    public Sprite star;
    [SerializeField] private Vector2 OffsetDirZ;
    [SerializeField] private Vector2 RangeSpeed;
    [SerializeField] private Vector2 RangeTimeSpawn = new Vector2(0.3f, 0.5f);


    private void Start()
    {

    }

    public void SetUp(Vector3 positionStart, Vector2 Dir, int Sl, float Dis)
    {
        for (int i = 0; i < Sl; i++)
        {
            Vector3 startPosition = positionStart + (Vector3)Dir * Dis * i;
            SpawnStars spawn = Instantiate(new GameObject()).AddComponent<SpawnStars>();
            spawn.SetUp(this, startPosition, new Vector2(45, 135), RangeSpeed, Random.Range(0.3f, 0.5f));
            OnSetSpawn += spawn.setSpawn;
        }
    }
    public void SetUp(Transform tf, Vector3 positionStart, Vector2 Dir, int Sl, float Dis)
    {
        for (int i = 0; i < Sl; i++)
        {
            Vector3 startPosition = positionStart + (Vector3)Dir * Dis * i;
            SpawnStars spawn = Instantiate(new GameObject()).AddComponent<SpawnStars>();
            spawn.SetUp(this, startPosition, OffsetDirZ, RangeSpeed, Random.Range(RangeTimeSpawn.x, RangeTimeSpawn.y));
            OnSetSpawn += spawn.setSpawn;
            spawn.transform.parent = tf;
        }
    }

    public void StartSpawn()
    {
        OnSetSpawn?.Invoke(true);
    }

    public void EndSpawn()
    {
        OnSetSpawn?.Invoke(false);
    }

    public UnityAction<bool> OnSetSpawn;
}
