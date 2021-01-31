using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStars : MonoBehaviour
{
    Bui bui;
    StarsControl control;
    Vector2 RangeDirZ;
    private Vector3 Center;
    private Vector2 RangeSpeed;
    private float RangeTimeSpawn;
    private Sprite sprite;
    bool spawn;

    float timeStartSpawn;
    void Start()
    {
        transform.position = Center;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawn)
        {
            if (Time.time - timeStartSpawn >= RangeTimeSpawn)
            {
                Spawn();
                timeStartSpawn = Time.time;
            }
        }
    }

    public void SetUp(StarsControl control, Vector3 position, Vector2 RangeDirZ, Vector2 RangeSpeed, float RangeTimeSpawn)
    {
        this.bui = control.bui;
        this.sprite = control.star;
        Center = position;
        this.RangeDirZ = RangeDirZ;
        this.RangeSpeed = RangeSpeed;
        this.RangeTimeSpawn = RangeTimeSpawn;
    }

    public void Spawn()
    {
        Bui bui = Instantiate(this.bui, transform.position, Quaternion.identity);
        bui.SetUp(sprite, 1f, MathQ.RotationToDirection(new Vector3(0, 0, Random.Range(RangeDirZ.x, RangeDirZ.y)).z), Random.Range(RangeSpeed.x, RangeSpeed.y), 1f, new Color(1,1,1));
    }

    public void setSpawn(bool a)
    {
        if (spawn == false && a == true)
        {
            spawn = true;
            timeStartSpawn = Time.time;
        }
        if (spawn == true && a == false)
        {
            spawn = false;
        }
    }
}
